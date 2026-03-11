using UnityEngine;

public class RootControl : MonoBehaviour
{

    public static RootControl instance;
    private static bool isInDemoMode;

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
        isInDemoMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setDemoMode(bool state){
        isInDemoMode = state;
    }

    public bool getDemoMode(){
        return isInDemoMode;
    }
}
