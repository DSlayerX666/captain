using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string[] randomEventsDescription;

    int health = 0;
    int sanity = 0;
    public int attack = 0;
    int defense = 0;
    int observe = 0;
    int speed = 0;
    public TMP_Text healthText;
    int currentHealth;
    public TMP_Text sanityText;
    int currentSanity;

    public TMP_Text timeText;
    public TMP_Text locationText;
    public GameObject[] buttons;
    public GameObject[] boatActivityButtons;

    // Awake is called before Start()
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = PlayerPrefs.GetInt("Health", 0);
        sanity = PlayerPrefs.GetInt("Sanity", 0);
        attack = PlayerPrefs.GetInt("Attack", 0);
        defense = PlayerPrefs.GetInt("Defense", 0);
        observe = PlayerPrefs.GetInt("Observe", 0);
        speed = PlayerPrefs.GetInt("Speed", 0);
        Debug.Log("Health: " + health);
        Debug.Log("Sanity: " + sanity);
        Debug.Log("Attack: " + attack);
        Debug.Log("Defense: " + defense);
        Debug.Log("Observe: " + observe);
        Debug.Log("Speed: " + speed);

        currentHealth = health;
        currentSanity = sanity;

        healthText.text = "ÑªÁ¿£º" + currentHealth + "/" + health;
        sanityText.text = "ÀíÖÇ£º" + currentSanity + "/" + sanity;
    }

    public void UpdateHealth(int dmg)
    {
        currentHealth -= dmg;
        healthText.text = "ÑªÁ¿£º" + currentHealth + "/" + health;
    }
}
