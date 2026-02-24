using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ButtonSound : MonoBehaviour
{
    AudioSource buttonFx;

    Button button;
    Toggle toggle;

    public AudioClip hoverFx;
    public AudioClip clickFx;

    [Tooltip("For toggles: play sound only when toggled ON")]
    public bool playToggleOnOnly = false;

    void Awake()
    {
        button = GetComponent<Button>();
        toggle = GetComponent<Toggle>();

        if (button)
        {
            button.onClick.AddListener(PlayClickSound);
        }

        if (toggle)
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    void OnDestroy()
    {
        if (button)
            button.onClick.RemoveListener(PlayClickSound);

        if (toggle)
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    // --- Hover (called from EventTrigger or IPointerEnter) ---
    public void HoverSound()
    {
        if (!IsInteractable() || !IsEnabled()) return;

        buttonFx = GetComponent<AudioSource>();
        buttonFx.volume = PlayerPrefs.GetFloat("Audio_SoundVolume", 0.5f);

        if (hoverFx && button.enabled == true) buttonFx.PlayOneShot(hoverFx);
    }

    // --- Button click ---
    public void PlayClickSound()
    {
        if (!IsInteractable() || !IsEnabled()) return;

        buttonFx = GetComponent<AudioSource>();
        buttonFx.volume = PlayerPrefs.GetFloat("Audio_SoundVolume", 0.5f);

        if (clickFx) buttonFx.PlayOneShot(clickFx);
    }

    // --- Toggle click ---
    void OnToggleChanged(bool isOn)
    {
        if (!IsInteractable() || !IsEnabled()) return;

        if (playToggleOnOnly && !isOn)
            return;

        buttonFx = GetComponent<AudioSource>();
        if (clickFx) buttonFx.PlayOneShot(clickFx);
    }

    bool IsInteractable()
    {
        if (button) return button.interactable;
        if (toggle) return toggle.interactable;
        return false;
    }

    bool IsEnabled()
    {
        if (button) return button.enabled;
        if (toggle) return toggle.enabled;
        return false;
    }
}
