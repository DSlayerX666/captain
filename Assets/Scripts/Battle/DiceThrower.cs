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

    public bool IsRunning { get; private set; } = false;

    private Dice[] _thrownDices;

    public int Result { get; private set; }

    /*private void Update()
    {
        if (IsRunning)
            return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(ThrowDicesCO(_dicesToThrow));
        }
    }*/

    public void StartDiceThrow(SDice[] dicesToThrow)
    {
        StartCoroutine(ThrowDicesCO(dicesToThrow));
    }

    private IEnumerator ThrowDicesCO(SDice[] dicesToThrow)
    {
        IsRunning = true;

        CanvasGroup.DOFade(1, 0.25f);

        _thrownDices = new Dice[dicesToThrow.Length];

        for (int i = 0; i < _thrownDices.Length; i++)
        {
            _thrownDices[i] = Instantiate(_dicePrefab, _diceParentRectTransform);
            _thrownDices[i].SetDiceSettings(dicesToThrow[i]);
            _thrownDices[i].transform.localScale = Vector3.zero;
            _thrownDices[i].transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
            _thrownDices[i].StartRoll();
            while (!_thrownDices[i].Finished)
            {
                yield return null;
            }
        }

        Result = 0;
        for (int i = 0; i < _thrownDices.Length; i++)
        {
            Result += _thrownDices[i].DiceNumber;
        }

        yield return WaitHandler.GetWaitForSeconds(1.5f);

        ClearDices();
    }

    public void ClearDices()
    {
        for (int i = 0; i < _thrownDices.Length; i++)
        {
            //_thrownDices[i].RectTransform.DOAnchorPosY(_thrownDices[i].RectTransform.anchoredPosition.y + 50, 0.25f);
            _thrownDices[i].RectTransform.DOScale(1.5f, 0.25f);
        }

        CanvasGroup.DOFade(0, 0.25f).OnComplete(DestroyDices);
    }

    private void DestroyDices()
    {
        for (int i = 0; i < _thrownDices.Length; i++)
        {
            Destroy(_thrownDices[i].gameObject);
        }
        IsRunning = false;
    }
}
