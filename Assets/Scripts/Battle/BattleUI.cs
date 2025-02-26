using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BattleUI : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 1.0f;
    [SerializeField] private float _fadeOutDuration = 1.0f;
    [SerializeField] private Ease _fadeEaseType = Ease.OutSine;

    public RectTransform RectTransform { get; private set; }
    public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float customDuration = -1)
    {
        gameObject.SetActive(true);

        if (customDuration >= 0)
            CanvasGroup.DOFade(1f, customDuration).SetEase(_fadeEaseType);
        else
            CanvasGroup.DOFade(1f, _fadeInDuration).SetEase(_fadeEaseType);
    }

    public void FadeOut(float customDuration = -1)
    {
        if (customDuration >= 0)
            CanvasGroup.DOFade(0f, customDuration).SetEase(_fadeEaseType).OnComplete(() => gameObject.SetActive(false));
        else
            CanvasGroup.DOFade(0f, _fadeOutDuration).SetEase(_fadeEaseType).OnComplete(() => gameObject.SetActive(false));
    }
}
