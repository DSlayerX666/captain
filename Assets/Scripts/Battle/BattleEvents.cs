using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EBattleEventType
{
    Attack = 0
}

public class BattleEvents : MonoBehaviour
{
    [SerializeField] private TMP_Text _battleText;
    [SerializeField] private BattleManager _battleManager;

    private void OnEnable()
    {
        _battleManager.OnAttackAction += AttackEvent;
    }

    private void OnDisable()
    {
        _battleManager.OnAttackAction -= AttackEvent;
    }

    public void AttackEvent(int damage, string attackerName, string defenderName)
    {
        _battleText.text = $"{attackerName} 发动了攻击！";
        _battleText.text += $"\n{defenderName} 受到了 {damage} 点伤害！";
    }

}
