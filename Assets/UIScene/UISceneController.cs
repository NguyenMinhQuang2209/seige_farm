using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneController : MonoBehaviour
{
    public string sceneName = "Main";
    [SerializeField] private GameObject howtoPlay;
    [SerializeField] private GameObject flag;
    private void Start()
    {
        CloseHowToPlay();
        if (flag != null)
        {
            flag.SetActive(false);
        }
    }
    public void CloseApplication()
    {
        Application.Quit();
    }
    public void ChangeScene()
    {
        flag.SetActive(true);
        Invoke(nameof(LoadScene), 4f);
    }
    public void OpenHowToPlay()
    {
        if (howtoPlay != null)
        {
            howtoPlay.SetActive(true);
        }
    }
    public void CloseHowToPlay()
    {
        if (howtoPlay != null)
        {
            howtoPlay.SetActive(false);
        }
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
