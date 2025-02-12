using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManagerNew : MonoBehaviour
{
    [Header("数值")]
    [SerializeField] private int minTestPlayerDamage = 10;
    [SerializeField] private int maxTestPlayerDamage = 25;
    [SerializeField] private int minTestEnemyDamage = 10;
    [SerializeField] private int maxTestEnemyDamage = 25;
    [SerializeField] private float testFleeSuccessChance = 0.5f;

    [Header("Settings")]
    [SerializeField] private float _delayAfterEachAction = 1f;

    [Header("Dependencies")]
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _runButton;
    [SerializeField] private Image _playerHealthBar;
    [SerializeField] private Image _enemyHealthBar;
    [SerializeField] private TMP_Text _playerHealthText;
    [SerializeField] private TMP_Text _enemyHealthText;
    [SerializeField] private TMP_Text _battleResultText;
    [SerializeField] private GameObject _actionsUI;

    int _currentPlayerHealth = 0;
    int _currentEnemyHealth = 0;

    public event Action<int, string, string> OnAttackAction;

    private void Awake()
    {
        _attackButton.onClick.AddListener(PlayerAttack);
        _runButton.onClick.AddListener((PlayerRun));
        _actionsUI.SetActive(false);
        _battleResultText.gameObject.SetActive(false);
    }

    private void Start()
    {
        //Debug
        StartBattle();
    }

    private void StartBattle()
    {
        _currentEnemyHealth = 100;
        _enemyHealthBar.fillAmount = _currentEnemyHealth / 100;
        _enemyHealthText.text = "血量： " + _currentEnemyHealth;

        _currentPlayerHealth = 100;
        _playerHealthBar.fillAmount = _currentPlayerHealth / 100;
        _playerHealthText.text = "血量： " + _currentPlayerHealth;

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        _actionsUI.SetActive(true);
    }

    private void PlayerAttack()
    {
        _actionsUI.SetActive(false);

        int damage = Random.Range(minTestPlayerDamage, maxTestPlayerDamage);
        bool enemyDead = EnemyTakeDamage(damage);
        OnAttackAction?.Invoke(damage, "玩家", "敌人");

        if (!enemyDead)
        {
            StartCoroutine(WaitBeforeNextAction(_delayAfterEachAction, EnemyAttack));
        }
        else
        {
            ShowBattleResult(EBattleResult.Win);
        }
    }

    private void EnemyAttack()
    {
        int damage = Random.Range(minTestEnemyDamage, maxTestEnemyDamage);
        bool playerDead = PlayerTakeDamage(damage);
        OnAttackAction?.Invoke(damage, "敌人", "玩家");

        if (!playerDead)
        {
            StartCoroutine(WaitBeforeNextAction(_delayAfterEachAction, StartPlayerTurn));
        }
        else
        {
            ShowBattleResult(EBattleResult.Lose);
        }
    }

    private void PlayerRun()
    {
        _actionsUI.SetActive(false);

        if (Random.Range(0f, 1f) > testFleeSuccessChance)
        {
            ShowBattleResult(EBattleResult.Fled);
        }
        else
        {
            //battle continue
            StartCoroutine(WaitBeforeNextAction(_delayAfterEachAction, EnemyAttack));
        }
    }

    private bool EnemyTakeDamage(int amount)
    {
        _currentEnemyHealth -= amount;
        _enemyHealthBar.fillAmount = (float)_currentEnemyHealth / 100;
        _enemyHealthText.text = "血量： " + _currentEnemyHealth;

        return _currentEnemyHealth <= 0;
    }

    private bool PlayerTakeDamage(int amount)
    {
        _currentPlayerHealth -= amount;
        _playerHealthBar.fillAmount = (float)_currentPlayerHealth / 100;
        _playerHealthText.text = "血量： " + _currentPlayerHealth;

        return _currentPlayerHealth <= 0;
    }

    private void ShowBattleResult(EBattleResult battleResult)
    {
        _actionsUI.SetActive(false);

        switch (battleResult)
        {
            case EBattleResult.Win:
                _battleResultText.text = "胜利！";
                _battleResultText.gameObject.SetActive(true);
                break;
            case EBattleResult.Lose:
                _battleResultText.text = "你死了！";
                _battleResultText.gameObject.SetActive(true);
                break;
            case EBattleResult.Fled:
                _battleResultText.text = "逃跑成功！";
                _battleResultText.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private IEnumerator WaitBeforeNextAction(float delay, Action nextAction)
    {
        yield return new WaitForSeconds(delay);

        nextAction?.Invoke();
    }
}
