using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;

public enum BattleState
{
    START = 0,
    ALLY_TURN = 1,
    ENEMY_TURN = 2,
    BATTLE_WON = 3,
    BATTLE_LOST = 4,
    FLED = 5
}

public class BattleManagerNew : MonoBehaviour
{
    //[Header("TEST")]
    [SerializeField] private List<UnitSlot> _allySlots;
    [SerializeField] private List<UnitSlot> _enemySlots;

    [Space]
    [SerializeField] private RectTransform _unitSlots;
    [SerializeField] private RectTransform _actionsUI;
    [SerializeField] private TargetSelection _targetSelection;
    [SerializeField] private TMP_Text _battleText;
    [SerializeField] private Button _attackButton;

    public BattleState _battleState { get; private set; }

    private List<Unit> _allUnits;
    private List<Unit> _allies;
    private List<Unit> _enemies;

    private Queue<Unit> _actionTurns = new Queue<Unit>();
    private Unit _currentMovingUnit = null;

    private bool _actionCompleted;

    private void Start()
    {
#if UNITY_EDITOR
        List<Unit> debugAllies = new List<Unit>();
        List<Unit> debugEnemies = new List<Unit>();

        for (int i = 0; i < _allySlots.Count; i++)
        {
            Unit debugUnit = _allySlots[i].GetComponentInChildren<Unit>();
            if (debugUnit != null)
            {
                debugAllies.Add(debugUnit);
                _allySlots[i].Initialize(debugUnit, EFaction.Ally);
            }
            else
                break;
        }

        for (int i = 0; i < _enemySlots.Count; i++)
        {
            Unit debugUnit = _enemySlots[i].GetComponentInChildren<Unit>();
            if (debugUnit != null)
            {
                debugEnemies.Add(debugUnit);
                _enemySlots[i].Initialize(debugUnit, EFaction.Enemy);
            }
            else
                break;
        }

        _allies = debugAllies;
        _enemies = debugEnemies;
        _allUnits = new List<Unit>();
        _allUnits.AddRange(_allies);
        _allUnits.AddRange(_enemies);

        StartBattle();
#endif
    }

    public void AssignUnits(List<Unit> allies, List<Unit> enemies)
    {
        _allies = allies;
        _enemies = enemies;

        for (int i = 0; i < _allySlots.Count; i++)
        {
            if (i < _allies.Count)
            {
                Instantiate(_allies[i], _allySlots[i].transform);
                _allySlots[i].Initialize(_allies[i], EFaction.Ally);
            }
        }

        for (int i = 0; i < _enemySlots.Count; i++)
        {
            if (i < _enemies.Count)
            {
                Instantiate(_enemies[i], _enemySlots[i].transform);
                _enemySlots[i].Initialize(_enemies[i], EFaction.Enemy);
            }
        }

    }

    private void OnEnable()
    {
        _attackButton.onClick.AddListener(OnAttackPressed);
    }

    public void StartBattle()
    {
        if (_allies == null || _enemies == null)
        {
            Debug.LogError("Assign Units before starting battle!");
            return;
        }

        _battleState = BattleState.START;
        _battleText.text = "战斗开始！";
        _actionsUI.gameObject.SetActive(false);

        StartCoroutine(DelayedCall(1f, GetNextTurn));
    }

    private void GetNextTurn()
    {
        if (_actionTurns.Count == 0) //Battle just started or all units already had their turn
        {
            IEnumerable<Unit> actionOrder  = (_allUnits.OrderByDescending(unit => unit.Stats[StatType.Speed]));
            foreach (Unit unit in actionOrder)
                _actionTurns.Enqueue(unit);
        }
        
        _currentMovingUnit = _actionTurns.Dequeue();
        _battleState = _currentMovingUnit.faction == EFaction.Ally ? BattleState.ALLY_TURN : BattleState.ENEMY_TURN;

        if (_currentMovingUnit.faction == EFaction.Ally)
        {
            StartCoroutine(StartAllyTurn());
        }
        else
        {
            StartCoroutine(StartEnemyTurn());
        }
    }

    private IEnumerator StartAllyTurn()
    {
        _battleState = BattleState.ALLY_TURN;
        _actionCompleted = false;
        _battleText.text = "选择行动：\n";

        _actionsUI.gameObject.SetActive(true);

        while (!_actionCompleted)
        {
            yield return null;
        }

        CheckBattleState();
    }

    private IEnumerator StartEnemyTurn()
    {
        _battleState = BattleState.ENEMY_TURN;

        yield return WaitHandler.GetWaitForSeconds(1f);

        CheckBattleState();
    }

    private void CheckBattleState()
    {
        if (_allies.Count == 0) //All allies defeated
            StartCoroutine(BattleWon());
        else if (_enemies.Count == 0) //All enemies defeated
            StartCoroutine(BattleLost());
        else
            GetNextTurn();
    }

    private IEnumerator BattleWon()
    {
        _battleState = BattleState.BATTLE_WON;

        yield return null;
    }

    private IEnumerator BattleLost()
    {
        _battleState = BattleState.BATTLE_LOST;

        yield return null;
    }

    #region Ally Actions

    private void OnAttackPressed()
    {
        _actionsUI.gameObject.SetActive(false);
        StartCoroutine(StartTargetSelection());
    }

    private IEnumerator StartTargetSelection()
    {
        _targetSelection.StartTargetSelection(_enemies, _unitSlots);

        while (_targetSelection.TargetSelectionResult == TargetSelectionResult.Running)
        {
            yield return null;
        }

        if (_targetSelection.TargetSelectionResult == TargetSelectionResult.Success)
        {
            StartAttack(_targetSelection.SelectedTarget);
            StartCoroutine(DelayedCall(1f, () => _actionCompleted = true));
        }
        else
        {
            _actionsUI.gameObject.SetActive(true);
        }
    }

    public void StartAttack(Unit target)
    {
        target.TakeDamage(_currentMovingUnit.Stats[StatType.Attack] + Random.Range(1, 7) - target.Stats[StatType.Defense]);
        if (target.CurrentHealth <= 0)
        {
            _enemies.Remove(target);
            Destroy(target.gameObject);
        }
    }

    #endregion

    private IEnumerator DelayedCall(float seconds, Action callback)
    {
        yield return WaitHandler.GetWaitForSeconds(seconds);
        callback?.Invoke();
    }
}
