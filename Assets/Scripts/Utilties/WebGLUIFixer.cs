using Crystal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLUIFixer : MonoBehaviour
{
    [SerializeField] private SafeArea _safeArea;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _safeArea.enabled = false;
            _safeArea.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.93f);
        }
        else
        {
            _safeArea.enabled = true;
        }
    }
}
