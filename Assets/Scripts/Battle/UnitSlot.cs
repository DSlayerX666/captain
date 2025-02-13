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

    private Unit _currentUnit = null;
    public bool IsEmpty = true;


    public void Initialize(Unit unit)
    {
        _currentUnit = unit;

        _unitNameText.text = unit.name;
        _unitHealth.fillAmount = 1;
        _unitHealthText.text = "血量：" + unit.CurrentHealth.ToString();

        IsEmpty = false;
    }
}
