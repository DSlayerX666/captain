using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingUI : MonoBehaviour
{
    [SerializeField] private float _duration = 2f;
    [SerializeField] private bool _invert = false;

    private void Start()
    {
        transform.DORotate(Vector3.forward * 360f * (_invert ? -1 : 1), _duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
}
