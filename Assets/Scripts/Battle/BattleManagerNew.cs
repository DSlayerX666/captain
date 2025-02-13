using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum BattleState
{
    START = 0,
    PLAYER_TURN = 1,
    ENEMY_TURN = 2,
    BATTLE_WON = 3,
    BATTLE_LOST = 4,
}

public class BattleManagerNew : MonoBehaviour
{
    //[Header("TEST")]
    [SerializeField] private List<UnitSlot> _allySlots;
    [SerializeField] private List<UnitSlot> _enemySlots;
    [SerializeField] private TMP_Text _battleText;

    public BattleState state { get; private set; }

    private List<Unit> _allies;
    private List<Unit> _enemies;

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
                _allySlots[i].Initialize(debugUnit);
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
                _enemySlots[i].Initialize(debugUnit);
            }
            else
                break;
        }

        _allies = debugAllies;
        _enemies = debugEnemies;

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
                _allySlots[i].Initialize(_allies[i]);
            }
            else
                _allySlots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _enemySlots.Count; i++)
        {
            if (i < _enemies.Count)
            {
                Instantiate(_enemies[i], _enemySlots[i].transform);
                _enemySlots[i].Initialize(_enemies[i]);
            }
            else
                _enemySlots[i].gameObject.SetActive(false);
        }

    }

    public void StartBattle()
    {
        if (_allies == null || _enemies == null)
        {
            Debug.LogError("Assign Units before starting battle!");
            return;
        }

        state = BattleState.START;
        _battleText.text = "战斗开始！";
        

    }

}
