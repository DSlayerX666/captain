using UnityEngine;
using UnityEngine.UI;

public class BattleBGM : MonoBehaviour
{
    public static BattleBGM instance;

    AudioSource audioSource;
    public AudioClip[] battleBgm;
    public Image backgroundImage;
    public Image battleImage;
    public GameObject[] battleButtons;

    // Awake is called before Start()
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBattleBGM()
    {
        audioSource.clip = battleBgm[Random.Range(0, battleBgm.Length)];
        audioSource.Play();
    }

    public void StopBattleBGM()
    {
        audioSource.Stop();
    }
}
