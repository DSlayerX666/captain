using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public enum StatType
{
    Health,
    Sanity,
    Constitution,
    Attack,
    Defense,
    Speed
}

[Serializable]
public struct UnitSettings
{
    public string Name;

    public int Health;
    public int Sanity;
    public int Constitution;
    public int Attack;
    public int Defense;
    public int Speed;
}

public enum EFaction
{
    Ally = 0,
    Enemy = 1
}

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitSettings _unitSettings;

    public string Name { get; private set; }
    public int CurrentHealth { get; private set; }
    public Dictionary<StatType, int> Stats { get; private set; }
    public int Level
    {
        get
        {
            int level = 0;
            foreach (var stat in Stats)
            {
                level += stat.Value;
            }
            return level;
        }
    }
    public EFaction faction { get; set; }

    public Button button { get; private set; }
    public UnityEvent<Unit> OnUnitSelected;

    public event Action OnDeath;

    private void Awake()
    {
        Stats = new Dictionary<StatType, int>()
        {
            {StatType.Health, _unitSettings.Health},
            {StatType.Sanity, _unitSettings.Sanity},
            {StatType.Constitution, _unitSettings.Constitution},
            {StatType.Attack, _unitSettings.Attack},
            {StatType.Defense, _unitSettings.Defense},
            {StatType.Speed, _unitSettings.Speed}
        };

        name = _unitSettings.Name;
        CurrentHealth = Stats[StatType.Health];

        button = GetComponent<Button>();
        SetSelectable(false);
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnSelected);
    }

    private void OnSelected()
    {
        OnUnitSelected?.Invoke(this);
    }

    public void SetSelectable(bool selectable)
    {
        button.enabled = selectable;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
    }
}
