using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SearchManager : MonoBehaviour
{
    public Button cancelButton;
    public Button playButton;
    public Transform content;
    public Button buttonInstance;
    public TMP_InputField inputField;
    [Space]
    [SerializeField] private Scrollbar scrollbar;
    public ScrollRect scrollRect;
    [SerializeField] private Button scrollUpButton;
    [SerializeField] private Button scrollDownButton;

    [Tooltip("Scroll amount per click (0–1)")]
    [SerializeField] private float step = 0.1f;

    private Button highlightedButton;

    public void ScrollUp()
    {
        scrollRect.verticalNormalizedPosition =
            Mathf.Clamp01(scrollRect.verticalNormalizedPosition + step);
    }

    public void ScrollDown()
    {
        scrollRect.verticalNormalizedPosition =
            Mathf.Clamp01(scrollRect.verticalNormalizedPosition - step);
    }

    void HighlightButton(Button button)
    {
        if (highlightedButton == button)
            return;

        ClearHighlight();

        highlightedButton = button;

        var colors = button.colors;
        button.targetGraphic.color = colors.selectedColor;
    }

    void ExecuteHighlighted()
    {
        if (!highlightedButton) return;

        highlightedButton.onClick.Invoke();
        HighlightButton(highlightedButton);
    }

    void ClearHighlight()
    {
        if (!highlightedButton)
            return;

        var colors = highlightedButton.colors;
        highlightedButton.targetGraphic.color = colors.normalColor;
        highlightedButton = null;
    }

    private void OnScrollbarValueChanged(float value)
    {
        // Disable when limits reached
        scrollUpButton.interactable = value < 1f;
        scrollDownButton.interactable = value > 0f;
    }

    public void Start()
    {
        cancelButton.onClick.AddListener(() => {
            GetComponent<PlayMissionManager>().ToggleSearchWindow(false);
            GetComponent<PlayMissionManager>().SetLevelInfo(GetComponent<PlayMissionManager>().selectedLevelNum);
            foreach (var button in FindObjectsOfType<Button>())
                button.enabled = true;
        });

        playButton.onClick.AddListener(() => {
            ExecuteHighlighted();
            SceneManager.LoadScene("Loading"); 
        });

        // Listen to text changes
        inputField.onValueChanged.AddListener(FilterButtons);

        // Listen for scrollbar movement (drag, mouse wheel, buttons)
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

        // Initial state
        OnScrollbarValueChanged(scrollbar.value);

        scrollUpButton.onClick.AddListener(ScrollUp);
        scrollDownButton.onClick.AddListener(ScrollDown);
    }

    void OnDestroy()
    {
        inputField.onValueChanged.RemoveListener(FilterButtons);
        scrollbar.onValueChanged.RemoveListener(OnScrollbarValueChanged);
    }


    private void FilterButtons(string input)
    {
        string search = input.Trim().ToLowerInvariant();
        bool found = false;

        ClearHighlight();

        for (int i = 0; i < content.childCount; i++)
        {
            Transform t = content.GetChild(i);
            TMP_Text text = t.GetChild(0).GetComponent<TMP_Text>();
            if (!text) continue;

            bool matches =
                string.IsNullOrEmpty(search) ||
                text.text.ToLowerInvariant().Contains(search);

            t.gameObject.SetActive(matches);

            if (matches && !found)
            {
                var btn = t.GetComponent<Button>();
                if (btn && btn.interactable)
                {
                    HighlightButton(btn);
                    found = true;
                }
            }
        }

        playButton.interactable = found;
    }

    public void InitSearchElements()
    {
        var sm = GetComponent<StatisticsManager>();
        List<Mission> missionList = new List<Mission>();

        missionList = sm.GetMissionList(Game.gold, Type.beginner);
        foreach (Mission m in missionList)
            if(m.levelNumber <= PlayerPrefs.GetInt("QualifiedLevelBeginnerGold", 0) + 1)
                InstantiateButton(m, Color.black);

        missionList = sm.GetMissionList(Game.gold, Type.intermediate);
        foreach (Mission m in missionList)
            if (m.levelNumber <= PlayerPrefs.GetInt("QualifiedLevelIntermediateGold", 0) + 1)
                InstantiateButton(m, Color.black);

        missionList = sm.GetMissionList(Game.gold, Type.advanced);
        foreach (Mission m in missionList)
            if (m.levelNumber <= PlayerPrefs.GetInt("QualifiedLevelAdvancedGold", 0) + 1)
                InstantiateButton(m, Color.black);

        missionList = sm.GetMissionList(Game.gold, Type.custom);
        foreach (Mission m in missionList)
            InstantiateButton(m, Color.green);

        missionList = sm.GetMissionList(Game.platinum, Type.beginner);
        foreach (Mission m in missionList)
            if (m.levelNumber <= PlayerPrefs.GetInt("QualifiedLevelBeginnerPlatinum", 0) + 1)
                InstantiateButton(m, Color.black);

        missionList = sm.GetMissionList(Game.platinum, Type.intermediate);
        foreach (Mission m in missionList)
            if (m.levelNumber <= PlayerPrefs.GetInt("QualifiedLevelIntermediatePlatinum", 0) + 1)
                InstantiateButton(m, Color.black);

        missionList = sm.GetMissionList(Game.platinum, Type.advanced);
        foreach (Mission m in missionList)
            if (m.levelNumber <= PlayerPrefs.GetInt("QualifiedLevelAdvancedPlatinum", 0) + 1)
                InstantiateButton(m, Color.black);

        missionList = sm.GetMissionList(Game.platinum, Type.expert);
        foreach (Mission m in missionList)
            if (m.levelNumber <= PlayerPrefs.GetInt("QualifiedLevelExpertPlatinum", 0) + 1)
                InstantiateButton(m, Color.black);

        SortButtons();
    }

    public void SelectFirstButton()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).gameObject.activeInHierarchy)
            {
                HighlightButton(content.GetChild(i).gameObject.GetComponent<Button>());
                break;
            }
        }
    }

    void InstantiateButton(Mission m, Color color)
    {
        var mission = Instantiate(buttonInstance, content);
        mission.gameObject.SetActive(true);

        var text = mission.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        text.text = m.levelName;
        text.color = color;

        var btn = mission.GetComponent<Button>();

        // Click = run logic + clear highlight
        btn.onClick.AddListener(() =>
        {
            ClearHighlight();
            SetMissionInfo(m);
        });

        // Hover = move highlight
        var trigger = mission.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener(_ => HighlightButton(btn));
        trigger.triggers.Add(entry);
    }


    void SetMissionInfo(Mission m)
    {
        MissionInfo.instance.MissionPath = m.directory;
        MissionInfo.instance.missionName = m.missionName;
        MissionInfo.instance.time = m.time;
        MissionInfo.instance.levelName = m.levelName;
        MissionInfo.instance.description = m.description;
        MissionInfo.instance.startHelpText = m.startHelpText;
        MissionInfo.instance.level = m.levelNumber;
        MissionInfo.instance.artist = m.artist;
        MissionInfo.instance.goldTime = m.goldTime;
        MissionInfo.instance.ultimateTime = m.ultimateTime;
        MissionInfo.instance.alarmTime = m.alarmTime;
        MissionInfo.instance.hasEgg = m.hasEgg;

        string musicName = m.music;
        musicName = string.IsNullOrEmpty(musicName) ? string.Empty : Path.GetFileNameWithoutExtension(musicName.Trim());
        musicName = musicName.Replace(".ogg", "");
        MissionInfo.instance.music = musicName;

        string skyboxName = m.skyboxName;
        skyboxName = string.IsNullOrEmpty(skyboxName) ? "intermediate_sky" : skyboxName;
        MissionInfo.instance.skybox = Application.CanStreamedLevelBeLoaded(skyboxName) ? skyboxName : "intermediate_sky";
    }

    public void SortButtons()
    {
        var buttons = content.Cast<Transform>()
            .Select(t =>
            {
                TMP_Text text = t.GetChild(0).GetComponent<TMP_Text>();
                return new
                {
                    Transform = t,
                    Text = text.text,
                    Color = text.color
                };
            })
            .OrderBy(b => ColorPriority(b.Color))
            .ThenBy(b => b.Text, System.StringComparer.OrdinalIgnoreCase)
            .ToList();

        // Apply sibling order
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Transform.SetSiblingIndex(i);
        }
    }

    private int ColorPriority(Color c)
    {
        if (Approximately(c, Color.black)) return 0;
        if (Approximately(c, Color.green)) return 1;
        return 2; // anything else goes last
    }

    private bool Approximately(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
               Mathf.Approximately(a.g, b.g) &&
               Mathf.Approximately(a.b, b.b);
    }
}
