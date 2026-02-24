using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button helpButton;
    public Button optionsButton;
    public Button quitButton;
    public Button websiteButton;
    public GameObject quitImage;

    bool isQuitting;

    public void Start()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene("PlayMission"));
        helpButton.onClick.AddListener(() => SceneManager.LoadScene("HelpCredits"));
        optionsButton.onClick.AddListener(() => SceneManager.LoadScene("Options"));
        quitButton.onClick.AddListener(() => {
            quitImage.SetActive(true);
            isQuitting = true;
        });
        websiteButton.onClick.AddListener(() => Application.OpenURL("https://marbleblast.com/"));
    }

    private void Update()
    {
        if (isQuitting && Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
