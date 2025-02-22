using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;
using DG.Tweening;

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
    [SerializeField] private RectTransform _allySlotsHolder;
    [SerializeField] private RectTransform _enemySlotsHolder;
    [SerializeField] private RectTransform _actionsUI;
    [SerializeField] private TargetSelection _targetSelection;
    [SerializeField] private TMP_Text _battleText;
    [SerializeField] private Button _attackButton;

    public BattleState _currentBattleState { get; private set; }

    private List<Unit> _allUnits;
    private List<Unit> _allies;
    private List<Unit> _enemies;

    private List<Unit> _turnQueue = new List<Unit>();
    private int _currentTurnIndex = -1;
    private Unit _currentMovingUnit = null;

    private CanvasGroup _actionsUICanvasGroup;
    private bool _actionCompleted;

    private void Awake()
    {
        _actionsUICanvasGroup = _actionsUI.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _actionsUI.gameObject.SetActive(false);

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
                continue;
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
                continue;
        }

        _allies = debugAllies;
        _enemies = debugEnemies;
        _allUnits = new List<Unit>();
        _allUnits.AddRange(_allies);
        _allUnits.AddRange(_enemies);

        StartBattle();
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

        _currentBattleState = BattleState.START;
        _battleText.text = "战斗开始！";
        HideActionUI();

        StartCoroutine(DelayedCall(1f, GetNextTurn));
    }

    private void GetNextTurn()
    {
        if (_turnQueue.Count < 1 || _currentTurnIndex + 1 >= _turnQueue.Count)
        {
            //New round, refresh and queue all units by speed
            _currentTurnIndex = 0;
            IEnumerable<Unit> turnQueue = (_allUnits.OrderByDescending(unit => unit.Stats[StatType.Speed]));
            foreach (Unit unit in turnQueue)
                _turnQueue.Add(unit);
        }
        else
        {
            _currentTurnIndex++;
        }

        _currentMovingUnit = _turnQueue[_currentTurnIndex];
        _currentBattleState = _currentMovingUnit.faction == EFaction.Ally ? BattleState.ALLY_TURN : BattleState.ENEMY_TURN;

        if (_currentMovingUnit == null || _currentMovingUnit.CurrentHealth <= 0)
        {
            GetNextTurn();
            return;
        }
        
        if (_currentMovingUnit.faction == EFaction.Ally)
        {
            StartCoroutine(StartAllyTurn());
        }
        else
        {
            StartCoroutine(StartEnemyTurn());
        }
    }

    public void ShowActionUI()
    {
        _actionsUI.gameObject.SetActive(true);
        _actionsUICanvasGroup.interactable = true;
        _actionsUI.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutSine);
        _actionsUICanvasGroup.DOFade(1f, 0.5f);
    }

    public void HideActionUI()
    {
        _actionsUICanvasGroup.interactable = false;
        _actionsUI.DOAnchorPosX(200f, 0.5f).SetEase(Ease.OutSine);
        _actionsUICanvasGroup.DOFade(0f, 0.5f).OnComplete(() => _actionsUI.gameObject.SetActive(false));
    }

    private IEnumerator StartAllyTurn()
    {
        _currentBattleState = BattleState.ALLY_TURN;
        _actionCompleted = false;
        _battleText.text = "选择行动：\n";

        _currentMovingUnit.StartTurn();

        ShowActionUI();

        while (!_actionCompleted)
        {
            yield return null;
        }

        _currentMovingUnit.EndTurn();

        CheckBattleState();
    }

    private IEnumerator StartEnemyTurn()
    {
        _currentBattleState = BattleState.ENEMY_TURN;
        _currentMovingUnit.StartTurn();

        yield return WaitHandler.GetWaitForSeconds(1f);

        _currentMovingUnit.EndTurn();

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
        _currentBattleState = BattleState.BATTLE_WON;

        yield return null;
    }

    private IEnumerator BattleLost()
    {
        _currentBattleState = BattleState.BATTLE_LOST;

        yield return null;
    }

    #region Ally Actions

    private void OnAttackPressed()
    {
        HideActionUI();
        StartCoroutine(StartTargetSelection());
    }

    private IEnumerator StartTargetSelection()
    {
        _targetSelection.StartTargetSelection(_enemies, _enemySlotsHolder);

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
            ShowActionUI();
        }
    }

    public void StartAttack(Unit target)
    {
        _currentMovingUnit.Attack();

        target.TakeDamage(_currentMovingUnit.Stats[StatType.Attack] + Random.Range(1, 7) - target.Stats[StatType.Defense]);
        if (target.CurrentHealth <= 0)
        {
            target.Die();
            _enemies.Remove(target);
        }
    }

    #endregion

    private IEnumerator DelayedCall(float seconds, Action callback)
    {
        yield return WaitHandler.GetWaitForSeconds(seconds);
        callback?.Invoke();
    }
}
