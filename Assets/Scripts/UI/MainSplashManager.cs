using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainSplashManager : MonoBehaviour
{
    public Button aButton;
    public Material beginnerSkyMat;
    public Material intermediateSkyMat;
    public Material advancedSkyMat;

    void Awake(){
      RenderSettings.skybox = intermediateSkyMat;
                             // Update the ambient lighting and reflection probes to match the new skybox
                            DynamicGI.UpdateEnvironment();
      GameObject previewController = GameObject.Find("PreviewRoot");
      previewController.GetComponent<Preview>().setCurrentSky(intermediateSkyMat);
    }

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
