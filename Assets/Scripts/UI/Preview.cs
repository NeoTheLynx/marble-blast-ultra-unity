using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TS;
using System.Threading.Tasks;
//using MissionImporterPreview;

public class Preview : MonoBehaviour
{

    public List<Mission> previewMissions = new List<Mission>();
    public List<Mission> tempMissions = new List<Mission>();
    public static Preview instance;
    private static bool isInPreviewMode;
    public GameObject mainParent;
    public Material currentSky;
    public GameObject previewInteriorPrefab;

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

    public void setCurrentSky(Material csky){
        currentSky = csky;
    }

    public Material getCurrentSky(){
        return currentSky;
    }

    public void setIsInPreviewMode(bool state){
        isInPreviewMode = state;
        if(isInPreviewMode) {
            EnableAllChildren();
            //StartCoroutine(updatePreviewSky());
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
                tempMissions = MissionInfo.instance.missionsUltraBeginner;
                previewMissions.AddRange(tempMissions);
                tempMissions = MissionInfo.instance.missionsUltraIntermediate;
                previewMissions.AddRange(tempMissions);
                tempMissions = MissionInfo.instance.missionsUltraAdvanced;
                previewMissions.AddRange(tempMissions);
                tempMissions = MissionInfo.instance.missionsUltraCustom;
                previewMissions.AddRange(tempMissions);

                tempMissions.Clear();
                //GameObject previewInteriorPrefab = Resources.Load<GameObject>("Prefabs/Interior");
                GameObject previewGlass3Prefab = Resources.Load<GameObject>("Prefabs/Structures/glass_3_mbu");
                GameObject previewGlass6Prefab = Resources.Load<GameObject>("Prefabs/Structures/glass_6_mbu");
                GameObject previewGlass9Prefab = Resources.Load<GameObject>("Prefabs/Structures/glass_9_mbu");
                GameObject previewGlass12Prefab = Resources.Load<GameObject>("Prefabs/Structures/glass_12_mbu");
                GameObject previewGlass15Prefab = Resources.Load<GameObject>("Prefabs/Structures/glass_15_mbu");
                GameObject previewGlass18Prefab = Resources.Load<GameObject>("Prefabs/Structures/glass_18_mbu");
                GameObject begCloud = Resources.Load<GameObject>("Prefabs/Structures/cloudsbeginner");
                GameObject intCloud = Resources.Load<GameObject>("Prefabs/Structures/cloudsintermediate");
                GameObject advCloud = Resources.Load<GameObject>("Prefabs/Structures/cloudsadvanced");
                //Loop through missions to add them to mega scene
                //for (int index = 0; index < previewMissions.Count; index++)
                //for (int index = 0; index < 73; index++)
                for (int index = 0; index < previewMissions.Count; index++)
                {
                    Debug.Log("Adding Mission To Preview: " + (previewMissions[index].directory));
                    // Creates an empty GameObject named "MyEmptyObject"
                    GameObject emptyLevelObject = new GameObject("Level_" + (previewMissions[index].missionName));
                    emptyLevelObject.gameObject.tag = "PreviewLevelGroup";
                    MissionImporterPreview mip = emptyLevelObject.AddComponent<MissionImporterPreview>();

                    // The object is created at the origin (0, 0, 0) by default in some cases, 
                    // but it's good practice to set its position explicitly if needed.
                    emptyLevelObject.transform.position = Vector3.zero;
                    emptyLevelObject.transform.parent = mainParent.transform;

                    
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setInteriorPrefab(previewInteriorPrefab);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setGlass3Prefab(previewGlass3Prefab);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setGlass6Prefab(previewGlass6Prefab);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setGlass9Prefab(previewGlass9Prefab);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setGlass12Prefab(previewGlass12Prefab);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setGlass15Prefab(previewGlass15Prefab);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setGlass18Prefab(previewGlass18Prefab);

                    emptyLevelObject.GetComponent<MissionImporterPreview>().setBeginnerClouds(begCloud);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setIntermediateClouds(intCloud);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setAdvancedClouds(advCloud);
                    emptyLevelObject.GetComponent<MissionImporterPreview>().setThisMission(previewMissions[index].directory);
                    emptyLevelObject.gameObject.SetActive(false);


                    //emptyLevelObject.AddComponent(typeof(RigidBody));
                    //MissionInfo.instance.MissionPath = ;
                    //if(names[index] == "Dave")
                      //  return names[index];
                }
    }

    IEnumerator updatePreviewSky()
    {
        // An infinite loop to keep the coroutine running indefinitely
        while (true)
        {
            // Code to execute
            //Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
            //Debug.Log("Fired cannonball.");

            // Pause the coroutine for the specified duration
            RenderSettings.skybox = currentSky;
                             // Update the ambient lighting and reflection probes to match the new skybox
                            DynamicGI.UpdateEnvironment();
            yield return new WaitForSeconds(1);
        }
    }
}
