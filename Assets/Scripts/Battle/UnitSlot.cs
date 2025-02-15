using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _unitNameText;
    [SerializeField] private Image _unitHealth;
    [SerializeField] private TMP_Text _unitHealthText;

    public Unit CurrentUnit { get; private set; }
    public bool IsEmpty = true;


    public void Initialize(Unit unit, EFaction faction)
    {
        CurrentUnit = unit;
        CurrentUnit.faction = faction;

        _unitNameText.text = unit.name;
        _unitHealth.fillAmount = 1;
        _unitHealthText.text = "血量：" + unit.CurrentHealth.ToString();

        IsEmpty = false;
    }
}
