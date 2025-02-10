using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stat
{
    public Stat(int index)
    {
        this.index = index;
    }

    int index = 0;
    public int amount { get; private set; } = 0;

    public void AddPoint()
    {
        amount++;
    }
}

public class CreateCharacter : MonoBehaviour
{
    int health = 0;
    public TMP_Text healthText;
    int sanity = 0;
    public TMP_Text sanityText;
    int attack = 0;
    public TMP_Text attackText;
    int defense = 0;
    public TMP_Text defenseText;
    int observe = 0;
    public TMP_Text observeText;
    int speed = 0;
    public TMP_Text speedText;

    /*private Stat _health;
    private Stat _sanity;
    private Stat _attack;
    private List<Stat> _stats = new List<Stat>();

    private void Awake()
    {
        _health = new Stat(0);
        _sanity = new Stat(1);
        //...

        _stats.Add(_health);
        _stats.Add(_sanity);
        //...
    }*/

    int pointsLeft = 20;
    public TMP_Text pointsLeftText;
    public Button startGameButton;

    public void IncreaseStatPoint(int index)
    {
        if (pointsLeft == 0)
        {
            return;
        }
        else if (pointsLeft == 1)
        {
            startGameButton.gameObject.SetActive(true);
        }

        //_stats[index].AddPoint();

        if (index == 0)
        {
            if (health == 5)
            {
                return;
            }
            health++;
            healthText.text = health.ToString();
        }
        else if (index == 1)
        {
            if (sanity == 5)
            {
                return;
            }
            sanity++;
            sanityText.text = sanity.ToString();
        }
        else if (index == 2)
        {
            if (attack == 5)
            {
                return;
            }
            attack++;
            attackText.text = attack.ToString();
        }
        else if (index == 3)
        {
            if (defense == 5)
            {
                return;
            }
            defense++;
            defenseText.text = defense.ToString();
        }
        else if (index == 4)
        {
            if (observe == 5)
            {
                return;
            }
            observe++;
            observeText.text = observe.ToString();
        }
        else if (index == 5)
        {
            if (speed == 5)
            {
                return;
            }
            speed++;
            speedText.text = speed.ToString();
        }

        pointsLeft--;
        pointsLeftText.text = "剩余点数: " + pointsLeft;
    }

    public void DecreaseStatPoint(int index)
    {
        if (index == 0)
        {
            if (health == 0)
            {
                return;
            }
            health--;
            healthText.text = health.ToString();
        }
        else if (index == 1)
        {
            if (sanity == 0)
            {
                return;
            }
            sanity--;
            sanityText.text = sanity.ToString();
        }
        else if (index == 2)
        {
            if (attack == 0)
            {
                return;
            }
            attack--;
            attackText.text = attack.ToString();
        }
        else if (index == 3)
        {
            if (defense == 0)
            {
                return;
            }
            defense--;
            defenseText.text = defense.ToString();
        }
        else if (index == 4)
        {
            if (observe == 0)
            {
                return;
            }
            observe--;
            observeText.text = observe.ToString();
        }
        else if (index == 5)
        {
            if (speed == 0)
            {
                return;
            }
            speed--;
            speedText.text = speed.ToString();
        }

        pointsLeft++;
        pointsLeftText.text = "剩余点数: " + pointsLeft;
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.SetInt("Sanity", sanity);
        PlayerPrefs.SetInt("Attack", attack);
        PlayerPrefs.SetInt("Defense", defense);
        PlayerPrefs.SetInt("Observe", observe);
        PlayerPrefs.SetInt("Speed", speed);
        StartCoroutine(SceneTransition.instance.TransitionToScene("GameScene"));
    }

    public void RandomiseStatPoints()
    {
        health = Random.Range(2, 6);
        sanity = Random.Range(2, 6);
        attack = Random.Range(2, 6);
        defense = Random.Range(1, 6);
        observe = Random.Range(1, 6);
        speed = Random.Range(1, 6);

        var sum = health + sanity + attack + defense + observe + speed;
        if (sum < 20)
        {
            speed += 20 - sum;
        }
        else if (sum > 20)
        {
            speed -= sum - 20;
        }

        while (speed <= 0 || speed > 5)
        {
            health = Random.Range(1, 6);
            sanity = Random.Range(1, 6);
            attack = Random.Range(1, 6);
            defense = Random.Range(1, 6);
            observe = Random.Range(1, 6);
            speed = Random.Range(1, 6);

            sum = health + sanity + attack + defense + observe + speed;
            if (sum < 20)
            {
                speed += 20 - sum;
            }
            else if (sum > 20)
            {
                speed -= sum - 20;
            }
        }

        healthText.text = health.ToString();
        sanityText.text = sanity.ToString();
        attackText.text = attack.ToString();
        defenseText.text = defense.ToString();
        observeText.text = observe.ToString();
        speedText.text = speed.ToString();

        pointsLeft = 0;
        pointsLeftText.text = "剩余点数: 0";

        startGameButton.gameObject.SetActive(true);
    }
}
