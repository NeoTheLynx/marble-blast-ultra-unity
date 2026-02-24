using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomResolutionManager : MonoBehaviour
{
    public Button applyButton;
    public Button cancelButton;
    public Transform content;
    public Button buttonInstance;
    [Space]
    [SerializeField] private Scrollbar scrollbar;
    public ScrollRect scrollRect;
    [SerializeField] private Button scrollUpButton;
    [SerializeField] private Button scrollDownButton;

    [Tooltip("Scroll amount per click (0–1)")]
    [SerializeField] private float step = 0.1f;

    private Button highlightedButton;
    int width, height;

    void Start()
    {
        applyButton.onClick.AddListener(() => GetComponent<OptionsManager>().SetCustomResolution(width, height));
        cancelButton.onClick.AddListener(() => GetComponent<OptionsManager>().CancelCustomResolution());

        // Listen for scrollbar movement (drag, mouse wheel, buttons)
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

        // Initial state
        OnScrollbarValueChanged(scrollbar.value);

        scrollUpButton.onClick.AddListener(ScrollUp);
        scrollDownButton.onClick.AddListener(ScrollDown);

        var resolutions = Utils.Get16by9Resolutions();
        foreach(var res in resolutions)
            InstantiateButton(res.width, res.height);

        width = 1280;
        height = 720;
        
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

    void InstantiateButton(int _width, int _height)
    {
        var mission = Instantiate(buttonInstance, content);
        mission.gameObject.SetActive(true);

        var text = mission.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        text.text = _width + " " + _height;

        var btn = mission.GetComponent<Button>();

        // Click = run logic + clear highlight
        btn.onClick.AddListener(() =>
        {
            ClearHighlight();
            width = _width;
            height = _height;
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
}
