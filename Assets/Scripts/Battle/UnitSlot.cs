using DG.Tweening;
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
    [SerializeField] private GameObject _turnIndicator;

    public Unit CurrentUnit { get; private set; } = null;

    private void Start()
    {
        _turnIndicator.SetActive(false);

        RectTransform turnIndicatorRect = _turnIndicator.GetComponent<RectTransform>();
        turnIndicatorRect.DOPunchAnchorPos(Vector2.down * 25, 1f, 0, 0).SetLoops(-1, LoopType.Restart);
        turnIndicatorRect.DORotate(Vector3.up * 360f, 1, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void Initialize(Unit unit, EFaction faction)
    {
        gameObject.SetActive(true);

        CurrentUnit = unit;
        CurrentUnit.faction = faction;

        CurrentUnit.OnHealthChanged += UpdateHealth;
        CurrentUnit.OnTurnStart += OnUnitTurnStart;
        CurrentUnit.OnActionPerformed += OnUnitPerformAction;
        CurrentUnit.OnTurnEnd += OnUnitPerformAction;
        CurrentUnit.OnDeath += RemoveUnit;

        _unitNameText.text = CurrentUnit.name;
        _unitHealth.fillAmount = (float)CurrentUnit.CurrentHealth / (float)CurrentUnit.Stats[StatType.Health];
        _unitHealthText.text = "血量：" + CurrentUnit.CurrentHealth.ToString();
    }

    public void UpdateHealth()
    {
        _unitHealth.fillAmount = (float)CurrentUnit.CurrentHealth / (float)CurrentUnit.Stats[StatType.Health];
        _unitHealthText.text = "血量：" + CurrentUnit.CurrentHealth.ToString();
    }

    public void OnUnitTurnStart()
    {
        _turnIndicator.SetActive(true);
    }

    public void OnUnitPerformAction()
    {
        _turnIndicator.SetActive(false);
    }

    public void OnUnitTurnEnd()
    {
        _turnIndicator.SetActive(false);
    }

    public void RemoveUnit()
    {
        CurrentUnit.OnHealthChanged -= UpdateHealth;
        CurrentUnit.OnTurnStart -= OnUnitTurnStart;
        CurrentUnit.OnActionPerformed -= OnUnitPerformAction;
        CurrentUnit.OnTurnEnd -= OnUnitPerformAction;
        CurrentUnit.OnDeath -= RemoveUnit;

        gameObject.SetActive(false);

        CurrentUnit = null;
    }
}
