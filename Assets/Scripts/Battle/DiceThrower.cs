using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceThrower : MonoBehaviour
{
    [SerializeField] private Dice _dicePrefab;
    [SerializeField] private RectTransform _diceParentRectTransform;
    [SerializeField] private SDice[] _dicesToThrow;

    private CanvasGroup _canvasGroup;
    public CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();

    private bool _isRunning = false;
    private bool _currentDiceFinished = false;

    private Dice[] _thrownDices;

    private void Update()
    {
        if (_isRunning)
            return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            _isRunning = true;
            StartCoroutine(ThrowDices());
        }
    }

    public IEnumerator ThrowDices()
    {
        CanvasGroup.DOFade(1, 0.25f);

        _thrownDices = new Dice[_dicesToThrow.Length];

        for (int i = 0; i < _thrownDices.Length; i++)
        {
            _thrownDices[i] = Instantiate(_dicePrefab, _diceParentRectTransform);
            _thrownDices[i].SetDiceSettings(_dicesToThrow[i]);
            _thrownDices[i].transform.localScale = Vector3.zero;
            _thrownDices[i].transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
            _thrownDices[i].StartRoll();
            while (!_thrownDices[i].Finished)
            {
                yield return null;
            }
        }

        GetResult();

        yield return WaitHandler.GetWaitForSeconds(1.5f);

        ClearDices();
    }

    public void ClearDices()
    {
        for (int i = 0; i < _thrownDices.Length; i++)
        {
            _thrownDices[i].RectTransform.DOAnchorPosY(_thrownDices[i].RectTransform.anchoredPosition.y + 50, 0.25f);
        }

        CanvasGroup.DOFade(0, 0.25f).OnComplete(DestroyDices);
    }

    private void DestroyDices()
    {
        for (int i = 0; i < _thrownDices.Length; i++)
        {
            Destroy(_thrownDices[i].gameObject);
        }
        _isRunning = false;
    }


    public int GetResult()
    {
        int result = 0;

        for (int i = 0; i < _thrownDices.Length; i++)
        {
            result += _thrownDices[i].DiceNumber;
        }

        Debug.LogWarning($"TOTAL: {result}");
        return result;
    }
}
