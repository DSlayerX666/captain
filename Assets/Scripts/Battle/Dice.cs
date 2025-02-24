using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct SDice
{
    public Sprite DiceImage;
    public int MaxNumber;
}

public class Dice : MonoBehaviour
{
    [SerializeField] private float _diceDelay = 0.1f;
    [SerializeField] private float _diceDelayMultiplier = 1.05f;
    [SerializeField] private float _diceStopThreshold = 0.5f;
    [SerializeField] private TMP_Text _diceText;
    [SerializeField] private Image _diceImage;
    [SerializeField] private SDice _diceSettings;
    [SerializeField] private AudioClip _diceRollSfx;
    [SerializeField] private AudioClip _diceFinishedSfx;

    private RectTransform _rectTransform;
    public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();

    public int DiceNumber { get; private set; } = 1;
    private float _timeElapsed = 0f;

    public bool Finished { get; private set; } = false;

    public void SetDiceSettings(SDice settings)
    {
        _diceSettings = settings;
        _diceImage.sprite = settings.DiceImage;
    }

    public void StartRoll()
    {
        StartCoroutine(RollCO());
    }

    private IEnumerator RollCO()
    {
        Finished = false;
        _timeElapsed = 0f;

        if (_diceRollSfx != null)
            AudioManager.Instance.PlayGlobalSfx(_diceRollSfx, 1, 1, 0.15f);

        while (_diceDelay < _diceStopThreshold) 
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _diceDelay)
            {
                //Generate new number
                _timeElapsed = 0f;
                GenerateNewNumber();

                _diceDelay *= _diceDelayMultiplier;
            }

            yield return null;
        }

        Finished = true;

        if (_diceFinishedSfx != null)
            AudioManager.Instance.PlayGlobalSfx(_diceFinishedSfx);

        _diceText.rectTransform.DOPunchScale(Vector3.one * 1.25f, 0.25f);
    }

    private void GenerateNewNumber()
    {
        int newDiceNumber = DiceNumber;
        while (newDiceNumber == DiceNumber)
        {
            DiceNumber = Random.Range(1, _diceSettings.MaxNumber);
        }

        _diceText.text = DiceNumber.ToString();
    }
}
