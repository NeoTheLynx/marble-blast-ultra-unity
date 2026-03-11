using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuButtonList : MonoBehaviour
{
    public Button buttonPrefab; // Reference to your Button Prefab
    public Button buttonPrefabSpacer; // Reference to your Button Prefab
    public Transform contentParent; // Reference to the Content GameObject in your Scroll Rect
    //public Transform contentParentSp; // Reference to the Content GameObject in your Scroll Rect

    private List<KeyValuePair<int, string>> menuOptions = new List<KeyValuePair<int, string>>() { 
                new KeyValuePair<int, string>(0, "Single Player Game"),
                new KeyValuePair<int, string>(1, "Multiplayer Game"),
                new KeyValuePair<int, string>(1, "SPACER"),
                new KeyValuePair<int, string>(2, "Leaderboards"),
                new KeyValuePair<int, string>(3, "Achievements"),
                new KeyValuePair<int, string>(4, "Help & Options"),
                new KeyValuePair<int, string>(5, "Download Content"),
                new KeyValuePair<int, string>(6, "Exit Game"),
    };

    void Start()
    {   
        if(RootControl.instance.getDemoMode()){
            menuOptions = new List<KeyValuePair<int, string>>() { 
                new KeyValuePair<int, string>(0, "Play Demo"),
                new KeyValuePair<int, string>(0, "SPACER"),
                //new KeyValuePair<int, string>(1, "Help & Options"),
                new KeyValuePair<int, string>(2, "Exit Game"),
            };
        }
        PopulateMenuItems();
        GameObject previewController = GameObject.Find("PreviewRoot");
        Material updateCurrentSky = previewController.GetComponent<Preview>().getCurrentSky();
        RenderSettings.skybox = updateCurrentSky;
                             //Update the ambient lighting and reflection probes to match the new skybox
                           DynamicGI.UpdateEnvironment();
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
            Debug.Log("Adding Clicks For: " + menuItem.Key.ToString());
            newButton.onClick.AddListener(() => OnButtonClick(menuItem.Key));
        }
    }

    void OnButtonClick(int btnIndex)
    {
        //Debug.Log("Clicked on button for: " + btnIndex.ToString());
        if(RootControl.instance.getDemoMode()){
            switch(btnIndex)
            {
                case 0:
                    SceneManager.LoadScene("DifficultySelect");
                    break;

                case 1:
                    SceneManager.LoadScene("Options");
                    break;

                case 2:
                    Application.Quit();
                    break;     

                default:
                    Debug.Log("<color=yellow>No Menu Index</color>");
                    break;
                }
        } else {
            switch(btnIndex)
            {
                case 0:
                    SceneManager.LoadScene("DifficultySelect");
                    break;

                case 1:
                    MessageBox.instance.showMessageBox("Multiplayer Not Implemented");
                    Debug.Log("<color=red>Multiplayer Not Implemented</color>");
                    break;

                case 2:
                    MessageBox.instance.showMessageBox("Leaderboards Not Implemented");
                    Debug.Log("<color=red>Leaderboards Not Implemented</color>");
                    break;

                case 3:
                    MessageBox.instance.showMessageBox("Achievements Not Implemented");
                    Debug.Log("<color=red>Achivements Not Implemented</color>");
                    break;

                case 4:
                    SceneManager.LoadScene("Options");
                    break;

                case 5:
                    MessageBox.instance.showMessageBox("Download Content Not Implemented");
                    Debug.Log("<color=red>Download Content Not Implemented</color>");
                    break;

                case 6:
                    Application.Quit();
                    break;        

                default:
                    Debug.Log("<color=yellow>No Menu Index</color>");
                    break;
            }
        }
        
        // Add your specific game logic here
    }
}
