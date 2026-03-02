using UnityEngine;

public class PreviewMetaData : MonoBehaviour
{

    public string levelDifficulty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setLevelDifficutly(string dtype){
        levelDifficulty = dtype;
    }

    public string getLevelDifficulty(){
        return levelDifficulty;
    }
}
