using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequenceUI : BattleUI
{
    [field: SerializeField] public RectTransform AllySingleTargetSlot { get; private set; }
    [field: SerializeField] public RectTransform EnemySingleTargetSlot { get; private set; }
}
