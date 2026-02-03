using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class UltraMenuListManager : MonoBehaviour
{
    public Button buttonPrefab; // Reference to your Button Prefab
    public Transform contentParent; // Reference to the Content GameObject in your Scroll Rect

    private List<KeyValuePair<int, string>> menuOptions = new List<KeyValuePair<int, string>>() { 
                new KeyValuePair<int, string>(0, "Single Player Game"),
                new KeyValuePair<int, string>(1, "Multiplayer Game"),
                new KeyValuePair<int, string>(2, "Leaderboards"),
                new KeyValuePair<int, string>(3, "Achievements"),
                new KeyValuePair<int, string>(4, "Help & Options"),
                new KeyValuePair<int, string>(5, "Download Content"),
                new KeyValuePair<int, string>(6, "Exit Game"),
    };

    void Start()
    {   
        PopulateMenuItems();
    }

    void PopulateMenuItems() {
        foreach (var menuItem in menuOptions) {
            // Instantiate the button prefab as a child of the content parent
            Button newButton = Instantiate(buttonPrefab, contentParent);
            
            // Set the button's text (assuming a child Text component)
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = menuItem.Value;

            // Add a listener to the button's OnClick event
            Debug.Log("Adding Clicks For: " + menuItem.Key.ToString());
            newButton.onClick.AddListener(() => OnButtonClick(menuItem.Key));
        }
    }

    void OnButtonClick(int btnIndex)
    {
        Debug.Log("Clicked on button for: " + btnIndex.ToString());
        switch(btnIndex)
        {
            case 0:
                SceneManager.LoadScene("PlayMission");
                break;

            case 1:
                Debug.Log("<color=red>Multiplayer Not Implemented</color>");
                break;

            case 2:
                Debug.Log("<color=red>Leaderboards Not Implemented</color>");
                break;

            case 3:
                Debug.Log("<color=red>Achivements Not Implemented</color>");
                break;

            case 4:
                SceneManager.LoadScene("Options");
                break;

            case 5:
                Debug.Log("<color=red>Download Content Not Implemented</color>");
                break;

            case 6:
                Application.Quit();
                break;        

            default:
                Debug.Log("<color=yellow>No Menu Index</color>");
                break;
        }
        // Add your specific game logic here
    }
}
