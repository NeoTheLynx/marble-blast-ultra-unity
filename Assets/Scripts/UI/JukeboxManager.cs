using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JukeboxManager : MonoBehaviour
{
    public static JukeboxManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

        // Initial state
        OnScrollbarValueChanged(scrollbar.value);

        scrollUpButton.onClick.AddListener(ScrollUp);
        scrollDownButton.onClick.AddListener(ScrollDown);
        nextButton.onClick.AddListener(NextSong);
        prevButton.onClick.AddListener(PrevSong);
        stopButton.onClick.AddListener(Stop);
        playButton.onClick.AddListener(Play);

        audioSource.volume = PlayerPrefs.GetFloat("Audio_MusicVolume", 0.5f);

        jukeboxWindowOpen = false;
        isPlaying = true;
        InitSong();
    }

    public List<AudioClip> musics = new List<AudioClip>();
    public AudioSource audioSource;
    public GameObject jukeboxWindow;
    public Button prevButton;
    public Button nextButton;
    public Button playButton;
    public Button stopButton;
    public TextMeshProUGUI musicInfo;
    [Space]
    public Transform content;
    public Button buttonInstance;
    [SerializeField] private Scrollbar scrollbar;
    public ScrollRect scrollRect;
    [SerializeField] private Button scrollUpButton;
    [SerializeField] private Button scrollDownButton;
    [SerializeField] private float step = 0.1f;
    private Button highlightedButton;
    private bool isPlaying;
    private bool jukeboxWindowOpen;
    private string currentlyPlayingMusic;
    private int selectedIndex;
    private AudioClip selectedAudioClip;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            jukeboxWindowOpen = !jukeboxWindowOpen;
            jukeboxWindow.SetActive(jukeboxWindowOpen);
        }

        if (Input.GetKeyDown(KeyCode.F6))
            PrevSong();
        if (Input.GetKeyDown(KeyCode.F7))
            TogglePlayStop();
        if (Input.GetKeyDown(KeyCode.F8))
            NextSong();

    }

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

    private void OnScrollbarValueChanged(float value)
    {
        // Disable when limits reached
        scrollUpButton.interactable = value < 1f;
        scrollDownButton.interactable = value > 0f;
    }

    void InstantiateButton(AudioClip _ac)
    {
        var mission = Instantiate(buttonInstance, content);
        mission.gameObject.SetActive(true);

        var text = mission.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        text.text = _ac.name;

        var btn = mission.GetComponent<Button>();

        // Click = run logic + clear highlight
        btn.onClick.AddListener(() =>
        {
            ClearHighlight();
            selectedAudioClip = _ac;
        });

        // Hover = move highlight
        var trigger = mission.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener(_ => HighlightButton(btn));
        trigger.triggers.Add(entry);
    }

    void HighlightButton(Button button)
    {
        if (highlightedButton == button)
            return;

        ClearHighlight();

        highlightedButton = button;

        var colors = button.colors;
        button.targetGraphic.color = colors.selectedColor;

        button.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(0.9804f, 0.7843f, 0.5137f, 1f);
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
        highlightedButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = Color.black;

        highlightedButton = null;
    }

    public void InitSong()
    {
        musics = musics.OrderBy(c => c.name).ToList();

        foreach (AudioClip ac in musics)
            InstantiateButton(ac);
    }

    public void TogglePlayStop()
    {
        if (isPlaying)
            Stop();
        else
            Play();
    }

    public void Stop()
    {
        isPlaying = false;
        audioSource.Stop();

        if (musicInfo.text.Contains("Playing"))
            musicInfo.text = musicInfo.text.Replace("Playing", "Stopped");
    }

    public void Play()
    {
        isPlaying = true;
        PlayMusic(selectedAudioClip.name);

        if(musicInfo.text.Contains("Stopped"))
            musicInfo.text = musicInfo.text.Replace("Stopped", "Playing");
    }

    public void NextSong()
    {
        selectedIndex++;
        if (selectedIndex >= musics.Count)
            selectedIndex = 0;

        PlayMusic(musics[selectedIndex].name);
    }

    public void PrevSong()
    {
        selectedIndex--;
        if (selectedIndex < 0)
            selectedIndex = musics.Count - 1;

        PlayMusic(musics[selectedIndex].name);
    }

    public void PlayMusic(string name)
    {
        var selectedMusic = musics.FirstOrDefault(c => c != null && c.name.ToLower() == name.ToLower());

        if (selectedMusic != null)
        {
            currentlyPlayingMusic = name.ToLower();
        }
        else
        {
            PlayMusic(musics[Random.Range(0, musics.Count)].name);
            return;
        }

        audioSource.Stop();
        audioSource.clip = selectedMusic;
        audioSource.Play();

        selectedIndex = GetClipIndexByName(name);
        selectedAudioClip = musics[selectedIndex];

        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).Find("Text").GetComponent<TextMeshProUGUI>().text == name)
            {
                HighlightButton(content.GetChild(i).gameObject.GetComponent<Button>());
                break;
            }
        }

        musicInfo.text = "Title: " + CapitalizeFirst(name) + "\nPlaying";
    }

    public void PlayRandomMusic()
    {
        PlayMusic(musics[Random.Range(0, musics.Count)].name);
    }

    int GetClipIndexByName(string clipName)
    {
        return musics.FindIndex(c =>
            c != null &&
            string.Equals(c.name, clipName, System.StringComparison.OrdinalIgnoreCase)
        );
    }

    public string CapitalizeFirst(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }
}
