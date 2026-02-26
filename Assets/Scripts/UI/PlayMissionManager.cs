using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Mission
{
    public Sprite levelImage;
    public string directory;
    public int levelNumber;
    [Space]
    public int time;
    public string missionName;
    public string levelName;
    [TextArea(2, 10)] public string description;
    [TextArea(2, 10)] public string startHelpText;
    public string artist;
    public int goldTime;
    public int ultimateTime;
    public int alarmTime;
    public string music;
    public string skyboxName;
    public bool hasEgg;
}

public enum Type
{
    none,
    beginner,
    intermediate,
    advanced,
    expert,
    custom
}

public enum Game
{
    none,
    gold,
    platinum,
    ultra
}

public class PlayMissionManager : MonoBehaviour
{
    public List<Mission> missions = new List<Mission>();
    [Header("UI References")]
    public Image levelImage;
    public TextMeshProUGUI levelDescriptionText;
    public TextMeshProUGUI bestTimesText;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI timeToQualifyText;
    public GameObject notQualifiedText;
    public GameObject notQualifiedImage;
    public GameObject beginnerButton;
    public GameObject intermediateButton;
    public GameObject advancedButton;
    public GameObject expertButton;
    public GameObject customButton;
    public GameObject switchGameButton;
    public GameObject[] spaces;
    public Button prev;
    public Button next;
    public Button play;
    public Button home;
    public Image eggImage;
    public Sprite egg;
    public Sprite egg_nf;
    [Space]
    public GameObject marbleSelectWindow;
    public GameObject statisticsWindow;
    public GameObject achievementsWindow;
    public GameObject searchWindow;
    public Button marbleSelectButton;
    public Button statisticsButton;
    public Button achievementsButton;
    public Button searchButton;
    [Space]
    public bool debug = false;

    [HideInInspector] public int selectedLevelNum;
    public static Type currentlySelectedType = Type.none;
    public static Game selectedGame = Game.none;

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) PrevButton();
        if (Input.GetKeyDown(KeyCode.RightArrow)) NextButton();
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MainMenu");
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        marbleSelectWindow.SetActive(false);
        statisticsWindow.SetActive(false);
        achievementsWindow.SetActive(false);
        searchWindow.SetActive(false);

        marbleSelectButton.onClick.AddListener(() => 
        {
            foreach (var button in FindObjectsOfType<Button>())
                button.enabled = false;

            ToggleMarbleSelectWindow(true); 
        });
        statisticsButton.onClick.AddListener(() =>
        {
            foreach (var button in FindObjectsOfType<Button>())
                button.enabled = false;

            GetComponent<StatisticsManager>().InitStatistics();
            ToggleStatisticsWindow(true);
        });
        achievementsButton.onClick.AddListener(() =>
        {
            foreach (var button in FindObjectsOfType<Button>())
                button.enabled = false;

            ToggleAchievementWindow(true);
        });
        searchButton.onClick.AddListener(() =>
        {
            foreach (var button in FindObjectsOfType<Button>())
                button.enabled = false;

            ToggleSearchWindow(true);
            GetComponent<SearchManager>().SelectFirstButton();
            GetComponent<SearchManager>().scrollRect.verticalNormalizedPosition = 1f;
        });

        StartCoroutine(WaitUntilFinishLoading());
    }

    public void ToggleMarbleSelectWindow(bool _active) => marbleSelectWindow.SetActive(_active);
    public void ToggleStatisticsWindow(bool _active) => statisticsWindow.SetActive(_active);
    public void ToggleAchievementWindow(bool _active) => achievementsWindow.SetActive(_active);
    public void ToggleSearchWindow(bool _active) => searchWindow.SetActive(_active);

    IEnumerator WaitUntilFinishLoading()
    {
        while (MissionInfo.instance.missionsPlatinumBeginner == null || MissionInfo.instance.missionsPlatinumBeginner.Count == 0)
            yield return null;

        Time.timeScale = 1;

        if (selectedGame == Game.none)
            selectedGame = Game.platinum;

        beginnerButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (selectedGame == Game.platinum)
                LoadMissions(Type.beginner, Game.platinum);
            else if (selectedGame == Game.gold)
                LoadMissions(Type.beginner, Game.gold);
        });
        intermediateButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (selectedGame == Game.platinum)
                LoadMissions(Type.intermediate, Game.platinum);
            else if (selectedGame == Game.gold)
                LoadMissions(Type.intermediate, Game.gold);
        });
        advancedButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (selectedGame == Game.platinum)
                LoadMissions(Type.advanced, Game.platinum);
            else if (selectedGame == Game.gold)
                LoadMissions(Type.advanced, Game.gold);
        });
        expertButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (selectedGame == Game.platinum)
                LoadMissions(Type.expert, Game.platinum);
        });
        customButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (selectedGame == Game.gold)
                LoadMissions(Type.custom, Game.gold);
        });
        switchGameButton.GetComponent<Button>().onClick.AddListener(() => SwitchGame());


        home.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        prev.onClick.AddListener(PrevButton);
        next.onClick.AddListener(NextButton);
        play.onClick.AddListener(() => SceneManager.LoadScene("Loading"));

        if (currentlySelectedType == Type.none)
            currentlySelectedType = Type.beginner;

        LoadMissions(currentlySelectedType, selectedGame);

        if (selectedGame == Game.gold)
        {
            achievementsButton.gameObject.SetActive(false);

            if (currentlySelectedType == Type.custom)
                statisticsButton.gameObject.SetActive(false);

            expertButton.SetActive(false);
            customButton.SetActive(true);
        }
        else if (selectedGame == Game.platinum)
        {
            achievementsButton.gameObject.SetActive(true);
            statisticsButton.gameObject.SetActive(true);

            expertButton.SetActive(true);
            customButton.SetActive(false);
        }

        GetComponent<SearchManager>().InitSearchElements();
    }

    void SwitchGame()
    {
        if (selectedGame == Game.gold)
        {
            selectedGame = Game.platinum;
            expertButton.SetActive(true);
            customButton.SetActive(false);
            currentlySelectedType = Type.beginner;
            LoadMissions(Type.beginner, Game.platinum);

            achievementsButton.gameObject.SetActive(true);
            statisticsButton.gameObject.SetActive(true);
        }
        else if (selectedGame == Game.platinum)
        {
            selectedGame = Game.gold;
            expertButton.SetActive(false);
            customButton.SetActive(true);
            currentlySelectedType = Type.beginner;
            LoadMissions(Type.beginner, Game.gold);

            achievementsButton.gameObject.SetActive(false);

            if (currentlySelectedType == Type.custom)
                statisticsButton.gameObject.SetActive(false);
        }
    }

    void LoadMissions(Type difficulty, Game game)
    {
        if (difficulty == Type.beginner)
                missions = MissionInfo.instance.missionsUltraBeginner;
            else if (difficulty == Type.intermediate)
                missions = MissionInfo.instance.missionsUltraIntermediate;
            else if (difficulty == Type.advanced)
                missions = MissionInfo.instance.missionsUltraAdvanced;
            else if (difficulty == Type.custom)
                missions = MissionInfo.instance.missionsGoldCustom;
        // if (game == Game.gold)
        // {
        //     if (difficulty == Type.beginner)
        //         missions = MissionInfo.instance.missionsGoldBeginner;
        //     else if (difficulty == Type.intermediate)
        //         missions = MissionInfo.instance.missionsGoldIntermediate;
        //     else if (difficulty == Type.advanced)
        //         missions = MissionInfo.instance.missionsGoldAdvanced;
        //     else if (difficulty == Type.custom)
        //         missions = MissionInfo.instance.missionsGoldCustom;
        // }
        // else if (game == Game.platinum)
        // {
        //     if (difficulty == Type.beginner)
        //         missions = MissionInfo.instance.missionsPlatinumBeginner;
        //     else if (difficulty == Type.intermediate)
        //         missions = MissionInfo.instance.missionsPlatinumIntermediate;
        //     else if (difficulty == Type.advanced)
        //         missions = MissionInfo.instance.missionsPlatinumAdvanced;
        //     else if (difficulty == Type.expert)
        //         missions = MissionInfo.instance.missionsPlatinumExpert;
        // }

        if (difficulty == Type.custom)
            statisticsButton.gameObject.SetActive(false);
        else
            statisticsButton.gameObject.SetActive(true);

        currentlySelectedType = difficulty;

        int qualifiedLevel = PlayerPrefs.GetInt("QualifiedLevel" + CapitalizeFirst(currentlySelectedType.ToString()) + CapitalizeFirst(selectedGame.ToString()), 0);
        selectedLevelNum = PlayerPrefs.GetInt("SelectedLevel" + CapitalizeFirst(currentlySelectedType.ToString()) + CapitalizeFirst(selectedGame.ToString()), qualifiedLevel);
        if (selectedLevelNum < 0) selectedLevelNum = 0;
        if (selectedLevelNum >= missions.Count) selectedLevelNum = missions.Count - 1;

        SetLevelInfo(selectedLevelNum);
    }

    public void PrevButton()
    {
        next.interactable = true;
        selectedLevelNum--;
        if (selectedLevelNum <= 0)
        {
            selectedLevelNum = 0;
            prev.interactable = false;
        }
        SetLevelInfo(selectedLevelNum);
    }

    public void NextButton()
    {
        prev.interactable = true;
        selectedLevelNum++;
        if (selectedLevelNum >= missions.Count - 1)
        {
            selectedLevelNum = missions.Count - 1;
            next.interactable = false;
        }
        SetLevelInfo(selectedLevelNum);
    }

    IEnumerator ChangePreviewLevel(string missionfile){
        GameObject parentObject = GameObject.Find("PreviewRoot");
        GameObject previewCamera = GameObject.Find("PreviewCamera");  
        if (parentObject != null)
        {
            // true parameter includes inactive children
            Transform[] children = parentObject.GetComponentsInChildren<Transform>(true); 

            foreach (Transform child in children)
            {
                if (child.CompareTag("PreviewLevelGroup"))
                {
                    child.gameObject.SetActive(false);

                }
            }
            foreach (Transform child in children)
            {
                if (child.CompareTag("PreviewLevelGroup"))
                {
                     if(child.name == "Level_" + missionfile){
                         child.gameObject.SetActive(true);
                         GameObject previewCameraPosition = GameObject.Find("CameraSpawnSphereMarker");
                         previewCamera.gameObject.transform.position = previewCameraPosition.gameObject.transform.position;
                         previewCamera.gameObject.transform.rotation = previewCameraPosition.gameObject.transform.rotation;
                    }
                }
            }
        }
        yield return null;
    }

    public void SetLevelInfo(int number)
    {
        foreach (GameObject g in spaces)
            g.SetActive(missions.Count != 0);

        if (missions.Count == 0)
        {
            levelDescriptionText.gameObject.SetActive(false);

            levelImage.color = Color.clear;
            currentLevelText.text = "Level 0";

            notQualifiedImage.SetActive(true);
            notQualifiedText.SetActive(true);

            prev.interactable = false;
            next.interactable = false;
            play.interactable = false;

            bestTimesText.text = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                string _name = "Matan W.";
                float _time = -1;
                bestTimesText.text += (i + 1) + ". " + _name;
                bestTimesText.text += "\t" + Utils.FormatTime(_time) + "\n";
            }

            return;
        }

        //ChangePreviewLevel(missions[number].missionName);
        StartCoroutine(ChangePreviewLevel(missions[number].missionName));
        //Debug.Log();

        int qualifiedLevel = debug ? 9999 : PlayerPrefs.GetInt("QualifiedLevel" + CapitalizeFirst(currentlySelectedType.ToString()) + CapitalizeFirst(selectedGame.ToString()), 0);

        if (currentlySelectedType.ToString().ToLower().Equals("custom"))
            qualifiedLevel = 9999;

        play.interactable = (qualifiedLevel >= number);
        prev.interactable = true;
        next.interactable = true;

        int lastQualifiedLevel = number > qualifiedLevel ? qualifiedLevel : number;
        PlayerPrefs.SetInt("SelectedLevel" + CapitalizeFirst(currentlySelectedType.ToString()) + CapitalizeFirst(selectedGame.ToString()), lastQualifiedLevel);

        if (number >= missions.Count - 1)
            next.interactable = false;

        if (number <= 0)
            prev.interactable = false;

        levelDescriptionText.gameObject.SetActive(true);

        levelDescriptionText.text = missions[number].description + "\n" +
            "<b>Author:</b> " + missions[number].artist;

        if (missions[number].time != -1)
            timeToQualifyText.text = "Par Time: " + Utils.FormatTime(missions[number].time);
        else
            timeToQualifyText.text = string.Empty;

        RefreshTMPLayout(levelDescriptionText);
        RefreshTMPLayout(timeToQualifyText);

        levelImage.color = Color.white;

        if (missions[number].levelImage)
        {
            levelImage.sprite = missions[number].levelImage;
            levelImage.color = Color.white;
        }
        else
        {
            levelImage.sprite = null;
            levelImage.color = Color.clear;
        }

        currentLevelText.text = missions[number].levelName + " - " + CapitalizeFirst(currentlySelectedType.ToString()) + " Level " + (number + 1);

        notQualifiedImage.SetActive(qualifiedLevel < number);
        notQualifiedText.SetActive(qualifiedLevel < number);

        MissionInfo.instance.MissionPath = missions[number].directory;
        MissionInfo.instance.missionName = missions[number].missionName;
        MissionInfo.instance.time = missions[number].time;
        MissionInfo.instance.levelName = missions[number].levelName;
        MissionInfo.instance.description = missions[number].description;
        MissionInfo.instance.startHelpText = missions[number].startHelpText;
        MissionInfo.instance.level = missions[number].levelNumber;
        MissionInfo.instance.artist = missions[number].artist;
        MissionInfo.instance.goldTime = missions[number].goldTime;
        MissionInfo.instance.ultimateTime = missions[number].ultimateTime;
        MissionInfo.instance.alarmTime = missions[number].alarmTime;
        MissionInfo.instance.hasEgg = missions[number].hasEgg;

        string musicName = missions[number].music;
        musicName = string.IsNullOrEmpty(musicName) ? string.Empty : Path.GetFileNameWithoutExtension(musicName.Trim());
        musicName = musicName.Replace(".ogg", "");
        MissionInfo.instance.music = musicName;

        string skyboxName = missions[number].skyboxName;
        skyboxName = string.IsNullOrEmpty(skyboxName) ? "intermediate_sky" : skyboxName;
        MissionInfo.instance.skybox = Application.CanStreamedLevelBeLoaded(skyboxName) ? skyboxName : "intermediate_sky";

        bestTimesText.text = string.Empty;
        for (int i = 0; i < 3; i++)
        {
            string _name = PlayerPrefs.GetString(MissionInfo.instance.levelName + "_Name_" + i, "Matan W.");
            float _time = PlayerPrefs.GetFloat(MissionInfo.instance.levelName + "_Time_" + i, -1);
            bestTimesText.text += (i + 1) + ". " + _name;

            if (selectedGame == Game.gold && currentlySelectedType != Type.custom && _time < MissionInfo.instance.goldTime && _time != -1)
            {
                bestTimesText.text += "\t" + Utils.FormatTime(_time) + "<sprite name=\"gold\">\n";
            }
            else if (selectedGame == Game.gold && currentlySelectedType == Type.custom)
            {
                if (_time < MissionInfo.instance.ultimateTime && _time != -1)
                    bestTimesText.text += "\t" + Utils.FormatTime(_time) + "<sprite name=\"ultimate\">\n";
                else if (_time < MissionInfo.instance.goldTime && _time != -1)
                    bestTimesText.text += "\t" + Utils.FormatTime(_time) + "<sprite name=\"platinum\">\n";
                else
                    bestTimesText.text += "\t" + Utils.FormatTime(_time) + "\n";
            }
            else if (selectedGame == Game.platinum)
            {
                if (_time < MissionInfo.instance.ultimateTime && _time != -1)
                    bestTimesText.text += "\t" + Utils.FormatTime(_time) + "<sprite name=\"ultimate\">\n";
                else if (_time < MissionInfo.instance.goldTime && _time != -1)
                    bestTimesText.text += "\t" + Utils.FormatTime(_time) + "<sprite name=\"platinum\">\n";
                else
                    bestTimesText.text += "\t" + Utils.FormatTime(_time) + "\n";
            }
            else
            {
                bestTimesText.text += "\t" + Utils.FormatTime(_time) + "\n";
            }
        }

        if (missions[number].hasEgg)
        {
            eggImage.gameObject.SetActive(true);
            if (PlayerPrefs.GetInt(MissionInfo.instance.levelName + "_EasterEgg", 0) == 1)
                eggImage.sprite = egg;
            else
                eggImage.sprite = egg_nf;
        }
        else
        {
            eggImage.gameObject.SetActive(false);
        }
    }

    public void RefreshTMPLayout(TextMeshProUGUI tmp)
    {
        tmp.ForceMeshUpdate();
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            tmp.rectTransform
        );
    }

    public static string CapitalizeFirst(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }
}
