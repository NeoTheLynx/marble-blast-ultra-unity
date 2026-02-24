using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TS;
//using MissionImporterPreview;

public class Preview : MonoBehaviour
{

    public List<Mission> previewMissions = new List<Mission>();
    public List<Mission> tempMisisons = new List<Mission>();
    public static Preview instance;
    private static bool isInPreviewMode;
    public GameObject mainParent;

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

    public void Start()
    {
        isInPreviewMode = true;
        LoadAllMissions();
        //GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Audio_MusicVolume", 0.5f);
    }

    public void setIsInPreviewMode(bool state){
        isInPreviewMode = state;
        if(isInPreviewMode) {
            EnableAllChildren();
        }
        else {
            DisableAllChildren();
        }
    }

    public bool getIsInPreviewMode(){
        return isInPreviewMode;
    }

    public void DisableAllChildren()
    {
        // Loop through all child Transforms of the parent object
        foreach (Transform child in transform)
        {
            // Access the GameObject of the child and set it to inactive
            child.gameObject.SetActive(false);
        }
    }

    public void EnableAllChildren()
    {
        // Loop through all child Transforms of the parent object
        // Note: this loop will only work for children that are active in the hierarchy unless you use GetComponentsInChildren<Transform>(true)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    //Load All Missions Into Mega Mission
    void LoadAllMissions()
    {
                tempMisisons = MissionInfo.instance.missionsGoldBeginner;
                previewMissions.AddRange(tempMisisons);
                tempMisisons = MissionInfo.instance.missionsGoldIntermediate;
                previewMissions.AddRange(tempMisisons);
                tempMisisons = MissionInfo.instance.missionsGoldAdvanced;
                previewMissions.AddRange(tempMisisons);
                tempMisisons = MissionInfo.instance.missionsGoldCustom;
                previewMissions.AddRange(tempMisisons);

                tempMisisons.Clear();
                GameObject previewInteriorPrefab = Resources.Load<GameObject>("Prefabs/Interior");
                //Loop through missions to add them to mega scene
                for (int index = 0; index < previewMissions.Count; index++)
                {
                    Debug.Log("Adding Mission To Preview: " + (previewMissions[index].directory));
                    // Creates an empty GameObject named "MyEmptyObject"
                    GameObject emptyLevelObject = new GameObject("Level_" + (previewMissions[index].missionName));
                    MissionImporterPreview mip = emptyLevelObject.AddComponent<MissionImporterPreview>();

                    // The object is created at the origin (0, 0, 0) by default in some cases, 
                    // but it's good practice to set its position explicitly if needed.
                    emptyLevelObject.transform.position = Vector3.zero;
                    emptyLevelObject.transform.parent = mainParent.transform;

                    
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setInteriorPrefab(previewInteriorPrefab);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setThisMission(previewMissions[index].directory);


                    //emptyLevelObject.AddComponent(typeof(RigidBody));
                    //MissionInfo.instance.MissionPath = ;
                    //if(names[index] == "Dave")
                      //  return names[index];
                }
    }
}
