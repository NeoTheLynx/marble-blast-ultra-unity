using UnityEngine;
using System.Collections.Generic;

public class Translations : MonoBehaviour
{

    public static Translations instance;
    private Dictionary<string, string> stringTable = new Dictionary<string, string>();

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
        //Levels
        stringTable.Add("$Text::LevelName1", "Learning To Roll");
        stringTable.Add("$Text::LevelName2", "Moving Up");
        stringTable.Add("$Text::LevelName3", "Gem Collection");
        stringTable.Add("$Text::LevelName4", "Frictional Concerns");
        stringTable.Add("$Text::LevelName5", "Triple Gravity");
        stringTable.Add("$Text::LevelName6", "Bridge Crossing");
        stringTable.Add("$Text::LevelName7", "Bunny Slope");
        stringTable.Add("$Text::LevelName8", "First Flight");
        stringTable.Add("$Text::LevelName9", "Hazardous Climb");
        stringTable.Add("$Text::LevelName10", "Marble Melee Primer");
        stringTable.Add("$Text::LevelName21", "Pitfalls");
        stringTable.Add("$Text::LevelName12", "Gravity Helix");
        stringTable.Add("$Text::LevelName22", "Platform Party");
        stringTable.Add("$Text::LevelName34", "Early Frost");
        stringTable.Add("$Text::LevelName23", "Winding Road");
        stringTable.Add("$Text::LevelName27", "Skate Park");
        stringTable.Add("$Text::LevelName28", "Ramp Matrix");
        stringTable.Add("$Text::LevelName39", "Half-Pipe");
        stringTable.Add("$Text::LevelName82", "Jump Jump Jump!");
        stringTable.Add("$Text::LevelName48", "Upward Spiral");
        stringTable.Add("$Text::LevelName104", "Divergence");
        stringTable.Add("$Text::LevelName107", "Urban Jungle");
        //Help Texts
        //translationList.Add("", "Learning To Roll");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getValue(string key){
        string stringVal = "";
        if(stringTable.ContainsKey(key)){
            stringVal = stringTable[key];
        } else {
            stringVal = key;
        }
        return stringVal;
    }
}
