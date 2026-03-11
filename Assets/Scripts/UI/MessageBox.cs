using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public static MessageBox instance;
    public TextMeshProUGUI messageText;
    public Button messageOkButton;
    //private static bool isInDemoMode;

    void Awake()
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        messageOkButton.onClick.AddListener(() => hideMessageBox());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void showMessageBox(string message){
        messageText.text = message;
        this.gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void hideMessageBox(){
        this.gameObject.GetComponent<Canvas>().enabled = false;
    }
}
