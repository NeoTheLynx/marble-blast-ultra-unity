using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class DifficultyMenuButtonList : MonoBehaviour
{
    public Button buttonPrefab; // Reference to your Button Prefab
    public Button buttonPrefabSpacer; // Reference to your Button Prefab
    public Transform contentParent; // Reference to the Content GameObject in your Scroll Rect
    //public Transform contentParentSp; // Reference to the Content GameObject in your Scroll Rect
    public Button aButton;
    public Button bButton;
    public MissionInfo misinfo;

    private List<KeyValuePair<int, string>> menuOptions = new List<KeyValuePair<int, string>>() { 
                new KeyValuePair<int, string>(0, "Beginner Levels"),
                new KeyValuePair<int, string>(1, "Intermediate Levels"),
                new KeyValuePair<int, string>(2, "Advanced Levels"),
                new KeyValuePair<int, string>(2, "SPACER"),
                new KeyValuePair<int, string>(3, "Custom Levels"),
                new KeyValuePair<int, string>(3, "SPACER"),
                new KeyValuePair<int, string>(4, "Gem Hunt"),
    };

    void Start()
    {   
        PopulateMenuItems();
        GameObject previewController = GameObject.Find("PreviewRoot");
        Material updateCurrentSky = previewController.GetComponent<Preview>().getCurrentSky();
        RenderSettings.skybox = updateCurrentSky;
                             //Update the ambient lighting and reflection probes to match the new skybox
                           DynamicGI.UpdateEnvironment();
        aButton.onClick.AddListener(() => onA());  
        bButton.onClick.AddListener(() => onB());
        misinfo = GameObject.Find("MissionInfo").GetComponent<MissionInfo>();  
    }

    void onA(){
        Debug.Log("This button does nothing atm");
    }

    void onB(){
        SceneManager.LoadScene("MainMenu");
    }

    void PopulateMenuItems() {
        foreach (var menuItem in menuOptions) {
            // Instantiate the button prefab as a child of the content parent
            Button newButton;
            if(menuItem.Value == "SPACER"){
                newButton = Instantiate(buttonPrefabSpacer, contentParent);
            } else {
                newButton = Instantiate(buttonPrefab, contentParent);
                newButton.name = "menulistbtn_" + menuItem.Value;
            }
            
            
            
            // Set the button's text (assuming a child Text component)
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = menuItem.Value;

            // Add a listener to the button's OnClick event
            //Debug.Log("Adding Clicks For: " + menuItem.Key.ToString());
            newButton.onClick.AddListener(() => OnButtonClick(menuItem.Key));
        }
    }

    void OnButtonClick(int btnIndex)
    {
        Debug.Log("Clicked on button for: " + btnIndex.ToString());
        switch(btnIndex)
        {
            case 0:
                misinfo.setCurrentDifficulty("beginner");
                SceneManager.LoadScene("PlayMission");
                break;

            case 1:
                misinfo.setCurrentDifficulty("intermediate");
                SceneManager.LoadScene("PlayMission");
                break;

            case 2:
                misinfo.setCurrentDifficulty("advanced");
                SceneManager.LoadScene("PlayMission");
                break;

            case 3:
                misinfo.setCurrentDifficulty("custom");
                SceneManager.LoadScene("PlayMission");
                break;

            case 4:
                Debug.Log("<color=red>Gem Hunt Not Implemented</color>");
                //SceneManager.LoadScene("PlayMission");
                break;      

            default:
                Debug.Log("<color=yellow>No Menu Index</color>");
                break;
        }
        // Add your specific game logic here
    }
}
