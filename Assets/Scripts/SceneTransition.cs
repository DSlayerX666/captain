using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    Animator sceneTransition;
    //public TMP_Dropdown qualityDropdown;

    // Awake is called before Start()
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneTransition = GetComponent<Animator>();

        //if (qualityDropdown != null)
        //{
        //    qualityDropdown.value = QualitySettings.GetQualityLevel();
        //    qualityDropdown.RefreshShownValue();
        //}
    }

    public IEnumerator TransitionToScene(string nextScene)
    {
        sceneTransition.SetTrigger("Transition");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    //public void SetQuality(int qualityIndex)
    //{
    //    //AudioManager.instance.PlaySound(pressButtonSound, 0.7f);
    //    QualitySettings.SetQualityLevel(qualityIndex);
    //    Debug.Log(QualitySettings.GetQualityLevel());
    //}
}
