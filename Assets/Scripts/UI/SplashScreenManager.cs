using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SplashScreenManager : MonoBehaviour
{
    public Image image;
    public Sprite production;
    public Sprite presents;
    public Sprite title;

    private void Update()
    {
        //if(Input.GetKey(KeyCode.Mouse0))
          //  SceneManager.LoadScene("MainMenu");
    }

    public void Start()
    {
        StartCoroutine(InitiatePreviewServer());
        JukeboxManager.instance.PlayMusic("Tim Trance");

        image.sprite = presents;
        image.color = Color.clear;
        image.DOColor(Color.white, 0.5f).OnComplete(() => {
            DOVirtual.DelayedCall(2f, () =>
            {
                image.DOColor(Color.clear, 0.5f).OnComplete(() => {
                    image.sprite = production;
                    image.DOColor(Color.white, 0.5f).OnComplete(() => {
                        DOVirtual.DelayedCall(2f, () =>
                        {
                            image.DOColor(Color.clear, 0.5f).OnComplete(() => {
                                image.sprite = title;
                                image.DOColor(Color.white, 0.5f).OnComplete(() => {
                                    DOVirtual.DelayedCall(2f, () =>
                                    {
                                        image.DOColor(Color.clear, 0.5f).OnComplete(() => {
                                            //SceneManager.LoadScene("MainMenu");
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    IEnumerator InitiatePreviewServer()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Previews", LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
         SceneManager.LoadScene("MainMenu");
    }
}
