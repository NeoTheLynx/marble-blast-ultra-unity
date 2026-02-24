using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public Sprite[] achievSprite;
    public Image[] achievImage;
    public Button okayButton;

    private void Start()
    {
        okayButton.onClick.AddListener(() => {
            GetComponent<PlayMissionManager>().ToggleAchievementWindow(false);
            foreach (var button in FindObjectsOfType<Button>())
                button.enabled = true;
        });
        InitAchiev();
    }

    void InitAchiev()
    {
        var sm = GetComponent<StatisticsManager>();

        int totalBeginnerPlatinum = sm.GetTotalCompletion(Game.platinum, Type.beginner);
        int totalIntermediatePlatinum = sm.GetTotalCompletion(Game.platinum, Type.intermediate);
        int totalAdvancedPlatinum = sm.GetTotalCompletion(Game.platinum, Type.advanced);
        int totalExpertPlatinum = sm.GetTotalCompletion(Game.platinum, Type.expert);
        int platinumTimes = sm.GetTotalPlatinumGold(Game.platinum, Type.beginner) + sm.GetTotalPlatinumGold(Game.platinum, Type.intermediate) + sm.GetTotalPlatinumGold(Game.platinum, Type.advanced) + sm.GetTotalPlatinumGold(Game.platinum, Type.expert);
        int ultimateTimes = sm.GetTotalUltimate(Game.platinum, Type.beginner) + sm.GetTotalUltimate(Game.platinum, Type.intermediate) + sm.GetTotalUltimate(Game.platinum, Type.advanced) + sm.GetTotalUltimate(Game.platinum, Type.expert);
        int totalEasterEgg = PlayerPrefs.GetInt("EasterEggCollected", 0);

        if (totalBeginnerPlatinum >= 25)
            achievImage[0].sprite = achievSprite[0];
        if (totalIntermediatePlatinum >= 35)
            achievImage[1].sprite = achievSprite[1];
        if (totalAdvancedPlatinum >= 35)
            achievImage[2].sprite = achievSprite[2];
        if (totalExpertPlatinum >= 25)
            achievImage[3].sprite = achievSprite[3];
        if (platinumTimes >= 120)
            achievImage[4].sprite = achievSprite[4];
        if (ultimateTimes >= 120)
            achievImage[5].sprite = achievSprite[5];
        if (totalEasterEgg >= 1)
            achievImage[6].sprite = achievSprite[6];
        if (totalEasterEgg >= 120)
            achievImage[7].sprite = achievSprite[7];
    }
}
