using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainSplashManager : MonoBehaviour
{
    public Button aButton;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
          startGame();

        if(Input.GetKey(KeyCode.Return))
          startGame();

        if(Input.GetKey(KeyCode.A))
          startGame();
    }

    void startGame(){
        JukeboxManager.instance.PlayMusic("Tim Trance");
        SceneManager.LoadScene("MainMenu");
    }

    public void Start()
    {
        aButton.onClick.AddListener(() => startGame());
    }

}
