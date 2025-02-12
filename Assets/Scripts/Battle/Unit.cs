using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatType
{
    Health = 0,
    Sanity = 1,
    Attack = 2,
    Defense = 3,
    Speed = 4
}

[Serializable]
public struct UnitSettings
{
    public string Name;

    public int Health;
    public int Sanity;
    public int Attack;
    public int Defense;
    public int Speed;
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

    private void Awake()
    {
        Stats = new Dictionary<StatType, int>()
        {
            {StatType.Health, _unitSettings.Health},
            {StatType.Sanity, _unitSettings.Sanity},
            {StatType.Attack, _unitSettings.Attack},
            {StatType.Defense, _unitSettings.Defense},
            {StatType.Speed, _unitSettings.Speed}
        };

        name = _unitSettings.Name;
        CurrentHealth = Stats[StatType.Health];
    }
}
