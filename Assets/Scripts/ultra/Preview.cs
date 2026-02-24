using UnityEngine;

public class Preview : MonoBehaviour
{
    public static Preview instance;
    private static bool isInPreviewMode;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        isInPreviewMode = true;
        //GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Audio_MusicVolume", 0.5f);
    }

    public void setIsInPreviewMode(bool state){
        isInPreviewMode = state;
    }

    public bool getIsInPreviewMode(){
        return isInPreviewMode;
    }
}
