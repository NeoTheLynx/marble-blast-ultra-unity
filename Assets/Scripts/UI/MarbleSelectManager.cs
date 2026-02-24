using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MarbleType
{
    Default,
    Custom
}

public class MarbleSelectManager : MonoBehaviour
{
    public Button next;
    public Button prev;
    public Button select;
    public Button customMarble;
    public Button normalMarble;
    public TextMeshProUGUI marbleNameText;
    public GameObject defaultMarbleTitle;
    public GameObject customMarbleTitle;
    public GameObject[] defaultMarblePreview;
    public GameObject[] customMarblePreview;

    MarbleType marbleType;
    int selectedIndexDefault;
    int selectedIndexCustom;
    int maxDefaultMarble;
    int maxCustomMarble;
    string[] defaultMarbleNames;
    string[] customMarbleNames;

    private void Start()
    {
        defaultMarbleNames = new string[40]
           {
            "Staff's Original",
            "3D Marble",
            "Mid P",
            "Spade",
            "GMD Logo",
            "Textured Marble",
            "Golden Marble",
            "Rainbow Marble",
            "Brown Swirls",
            "Caution Stripes",
            "Earth",
            "Golf ball",
            "Jupiter",
            "MB Gold Marble",
            "MBP on the Marble!",
            "Moshe",
            "Strong Bad",
            "Venus",
            "Water",
            "Evil Eye",
            "Desert and Sky",
            "Dirt Marble",
            "Friction Textured Marble",
            "Grass",
            "Mars",
            "Phil's Golf ball",
            "Molten",
            "Perishingflames",
            "Phil'sEmpire",
            "Matan's Red Dragon",
            "Metallic Marble",
            "Sun",
            "Underwater",
            "GarageGames", //0.75
            "BM 1: Color Burst", //1
            "BM 2: Oil Spill", //1.25
            "BM 3: Fire!", //1.5
            "SM 1: Midget Marble", //0.3125
            "SM 2: Eerie Greenie", //0.25
            "SM 3: Mini Easter Egg!" //0.125
           };

        customMarbleNames = new string[31]
        {
            "CyberFox1",
            "CyberFox2",
            "CyberFox3",
            "CyberFox4",
            "CyberFox5",
            "CyberFox6",
            "CustomMarble_01",
            "CustomMarble_02",
            "CustomMarble_03",
            "CustomMarble_04",
            "CustomMarble_05",
            "CustomMarble_06",
            "CustomMarble_07",
            "CustomMarble_08",
            "CustomMarble_09",
            "CustomMarble_10",
            "CustomMarble_11",
            "CustomMarble_12",
            "CustomMarble_13",
            "CustomMarble_14",
            "CustomMarble_15",
            "CustomMarble_16",
            "CustomMarble_17",
            "CustomMarble_18",
            "CustomMarble_19",
            "CustomMarble_20",
            "CustomMarble_21",
            "CustomMarble_22",
            "CustomMarble_23",
            "CustomMarble_24",
            "CustomMarble_25",
        };

        marbleType = PlayerPrefs.GetInt("DefaultMarbleIsSelected", 0) == 0 ? MarbleType.Default : MarbleType.Custom;
        SwitchCategory();

        selectedIndexDefault = 0;
        selectedIndexCustom = 0;

        if (marbleType == MarbleType.Default)
            selectedIndexDefault = PlayerPrefs.GetInt("SelectedMarbleIndex", 0);
        else
            selectedIndexCustom = PlayerPrefs.GetInt("SelectedMarbleIndex", 0);

        maxDefaultMarble = 40;
        maxCustomMarble = 31;

        prev.onClick.AddListener(Prev);
        next.onClick.AddListener(Next);
        select.onClick.AddListener(CloseMarbleSelect);
        customMarble.onClick.AddListener(() => 
        {
            marbleType = MarbleType.Custom;
            SwitchCategory();
        });

        normalMarble.onClick.AddListener(() =>
        {
            marbleType = MarbleType.Default;
            SwitchCategory();
        });

        SelectMarble();
    }

    public void CloseMarbleSelect()
    {
        PlayerPrefs.SetInt("DefaultMarbleIsSelected", (marbleType == MarbleType.Default) ? 0 : 1);
        PlayerPrefs.SetInt("SelectedMarbleIndex", (marbleType == MarbleType.Default) ? selectedIndexDefault : selectedIndexCustom);

        foreach (var button in FindObjectsOfType<Button>())
                button.enabled = true;

        GetComponent<PlayMissionManager>().ToggleMarbleSelectWindow(false);
    }

    public void SwitchCategory()
    {
        if (marbleType == MarbleType.Default)
        {
            defaultMarbleTitle.SetActive(true);
            customMarbleTitle.SetActive(false);
            normalMarble.gameObject.SetActive(false);
            customMarble.gameObject.SetActive(true);
        }
        else
        {
            defaultMarbleTitle.SetActive(false);
            customMarbleTitle.SetActive(true);
            normalMarble.gameObject.SetActive(true);
            customMarble.gameObject.SetActive(false);
        }

        SelectMarble();
    }

    public void SelectMarble()
    {
        foreach (GameObject marble in defaultMarblePreview)
            marble.SetActive(false);
        foreach (GameObject marble in customMarblePreview)
            marble.SetActive(false);

        if (marbleType == MarbleType.Default)
        {
            marbleNameText.text = defaultMarbleNames[selectedIndexDefault];
            defaultMarblePreview[selectedIndexDefault].SetActive(true);
        }
        else
        {
            marbleNameText.text = customMarbleNames[selectedIndexCustom];
            customMarblePreview[selectedIndexCustom].SetActive(true);
        }
    }

    public void Next()
    {
        if (marbleType == MarbleType.Default)
        {
            selectedIndexDefault++;
            if (selectedIndexDefault >= maxDefaultMarble)
                selectedIndexDefault = 0;
        }
        else
        {
            selectedIndexCustom++;
            if (selectedIndexCustom >= maxCustomMarble)
                selectedIndexCustom = 0;
        }

        SelectMarble();
    }

    public void Prev()
    {
        if (marbleType == MarbleType.Default)
        {
            selectedIndexDefault--;
            if (selectedIndexDefault < 0)
                selectedIndexDefault = maxDefaultMarble - 1;
        }
        else
        {
            selectedIndexCustom--;
            if (selectedIndexCustom < 0)
                selectedIndexCustom = maxCustomMarble - 1;
        }

        SelectMarble();
    }

}
