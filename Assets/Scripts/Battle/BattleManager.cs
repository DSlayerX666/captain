using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    [Header("数值")]
    [SerializeField] private int minTestPlayerDamage = 10;
    [SerializeField] private int maxTestPlayerDamage = 25;
    [SerializeField] private int minTestEnemyDamage = 10;
    [SerializeField] private int maxTestEnemyDamage = 25;

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


    private void Awake()
    {
        _attackButton.onClick.AddListener(() => StartCoroutine(PlayerAttack()));
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

    private IEnumerator PlayerAttack()
    {
        _actionsUI.SetActive(false);

        int damage = Random.Range(minTestPlayerDamage, maxTestPlayerDamage);
        bool enemyDead = EnemyTakeDamage(damage);

        if (enemyDead)
            yield break;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        int damage = Random.Range(minTestEnemyDamage, maxTestEnemyDamage);
        bool playerDead = PlayerTakeDamage(damage);

        if (playerDead)
            yield break;

        yield return new WaitForSeconds(0.5f);

        StartPlayerTurn();
    }

    private void PlayerRun()
    {
        _actionsUI.SetActive(false);

        _battleResultText.text = "逃跑成功！";
        _battleResultText.gameObject.SetActive(true);
    }

    private bool EnemyTakeDamage(int amount)
    {
        _currentEnemyHealth -= amount;
        _enemyHealthBar.fillAmount = (float)_currentEnemyHealth / 100;
        _enemyHealthText.text = "血量： " + _currentEnemyHealth;

        if (_currentEnemyHealth <= 0)
        {
            _battleResultText.text = "胜利！";
            _battleResultText.gameObject.SetActive(true);
        }

        return _currentEnemyHealth <= 0;
    }

    private bool PlayerTakeDamage(int amount)
    {
        _currentPlayerHealth -= amount;
        _playerHealthBar.fillAmount = (float)_currentPlayerHealth / 100;
        _playerHealthText.text = "血量： " + _currentPlayerHealth;

        if (_currentPlayerHealth <= 0)
        {
            _battleResultText.text = "你死了！";
            _battleResultText.gameObject.SetActive(true);
        }
        return _currentPlayerHealth <= 0;
    }
}
