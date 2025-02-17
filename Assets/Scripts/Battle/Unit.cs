using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

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
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private AudioClip _attackSfx;
    [SerializeField] private AudioClip _damageSfx;
    [SerializeField] private AudioClip _deathSfx;

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

    private Material _originalMaterial;
    public RectTransform RectTransform { get; private set; }
    public Button Button { get; private set; }
    public UnityEvent<Unit> OnUnitSelected;

    public event Action OnHealthChanged;
    public event Action OnDeath;
    public event Action OnTurnStart;
    public event Action OnActionPerformed;
    public event Action OnTurnEnd;

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

        RectTransform = GetComponent<RectTransform>();
        Button = GetComponent<Button>();
        _originalMaterial = Button.image.material;

        SetSelectable(false);
    }

    private void OnEnable()
    {
        Button.onClick.AddListener(OnSelected);
    }

    private void OnSelected()
    {
        OnUnitSelected?.Invoke(this);
    }

    public void SetSelectable(bool selectable)
    {
        Button.enabled = selectable;
    }

    public void StartTurn()
    {
        OnTurnStart?.Invoke();
    }

    public void Attack()
    {
        AudioManager.Instance.PlayGlobalSfx(_attackSfx, 1, 1, 0.1f);

        OnActionPerformed?.Invoke();
    }

    public void EndTurn()
    {
        OnTurnEnd?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        OnHealthChanged?.Invoke();

        if (_damageSfx != null)
            AudioManager.Instance.PlayGlobalSfx(_damageSfx, 1, 1, 0.1f);

        //Effect
        RectTransform.DOShakeAnchorPos(0.5f, 20f, 100, 90, false, true);

        if (_flashMaterial != null) 
        {
            Button.image.material = _flashMaterial;
            DOVirtual.DelayedCall(0.1f, () => Button.image.material = _originalMaterial);
        }
    }

    public void Die()
    {
        if (_deathSfx != null) 
        {
            AudioManager.Instance.PlayGlobalSfx(_deathSfx);
        }

        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
