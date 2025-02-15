using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioClip _hoverSfx;
    [SerializeField] private AudioClip _pressSfx;

    private const float hoveredScale = 1.1f;
    private const float scaleDuration = 0.2f;
    private Tween _scaleTween = null;
    private RectTransform _rectTransform;
    private Button _button;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClicked);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_scaleTween != null)
            _scaleTween.Kill(true);

        _scaleTween = _rectTransform.DOScale(hoveredScale, scaleDuration);

        if (_hoverSfx != null)
            AudioManager.Instance.PlayGlobalSfx(_hoverSfx, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_scaleTween != null)
            _scaleTween.Kill(true);

        _scaleTween = _rectTransform.DOScale(1f, scaleDuration);
    }

    public void OnClicked()
    {
        if (_pressSfx != null)
            AudioManager.Instance.PlayGlobalSfx(_pressSfx, 1f);
    }
}
