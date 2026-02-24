using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsManager : MonoBehaviour
{
    public Button returnButton;
    public TextMeshProUGUI captions;
    public TextMeshProUGUI completionText;
    public TextMeshProUGUI percentageText;

    private void Start()
    {
        returnButton.onClick.AddListener(() => { 
            GetComponent<PlayMissionManager>().ToggleStatisticsWindow(false);
            foreach (var button in FindObjectsOfType<Button>())
                button.enabled = true;
        });
    }

    public void InitStatistics()
    {
        TotalTimeTracker.instance.SaveTotalTime();

        percentageText.text = string.Empty;
        captions.text = string.Empty;
        completionText.text = string.Empty;

        if (PlayMissionManager.selectedGame == Game.platinum)
        {
            int totalBeginnerPlatinum = GetTotalCompletion(Game.platinum, Type.beginner);
            int totalIntermediatePlatinum = GetTotalCompletion(Game.platinum, Type.intermediate);
            int totalAdvancedPlatinum = GetTotalCompletion(Game.platinum, Type.advanced);
            int totalExpertPlatinum = GetTotalCompletion(Game.platinum, Type.expert);
            int total = totalBeginnerPlatinum + totalIntermediatePlatinum + totalAdvancedPlatinum + totalExpertPlatinum;
            int platinumTimes = GetTotalPlatinumGold(Game.platinum, Type.beginner) + GetTotalPlatinumGold(Game.platinum, Type.intermediate) + GetTotalPlatinumGold(Game.platinum, Type.advanced) + GetTotalPlatinumGold(Game.platinum, Type.expert);
            int ultimateTimes = GetTotalUltimate(Game.platinum, Type.beginner) + GetTotalUltimate(Game.platinum, Type.intermediate) + GetTotalUltimate(Game.platinum, Type.advanced) + GetTotalUltimate(Game.platinum, Type.expert);
            long grandTotalPlatinum = GetTotalTime(Game.platinum);
            int totalEasterEgg = PlayerPrefs.GetInt("EasterEggCollected", 0);

            captions.text = "Beginner:\n" +
                "Intermediate:\n" +
                "Advanced:\n" +
                "Expert:\n" +
                "Total:\n" +
                "\n" +
                "Platinum times:\n" +
                "Ultimate times:\n" +
                "Easter Eggs:\n" +
                "Out of Bounds:\n" +
                "Grand Total (hours:minutes:seconds):\n" +
                "\n" +
                "Total Waster Time (days:hours:minutes:seconds):\n";

            completionText.text = "\n" +
                GetColor(Mathf.FloorToInt(platinumTimes / 120)) + platinumTimes + "/120</color>\n" +
                GetColor(Mathf.FloorToInt(ultimateTimes / 120)) + ultimateTimes + "/120</color>\n" +
                GetColor(Mathf.FloorToInt(totalEasterEgg / 120)) + totalEasterEgg + "/120</color>\n";

            int percentage = 0;

            percentage = Mathf.FloorToInt(totalBeginnerPlatinum * 100 / 25);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt(totalIntermediatePlatinum * 100 / 35);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt(totalAdvancedPlatinum * 100 / 35);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt(totalExpertPlatinum * 100 / 25);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt((totalBeginnerPlatinum + totalIntermediatePlatinum + totalAdvancedPlatinum + totalExpertPlatinum) * 100 / 120);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";

            percentageText.text += "\n";

            percentage = Mathf.FloorToInt(platinumTimes * 100 / 120);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt(ultimateTimes * 100 / 120);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt(totalEasterEgg * 100 / 120);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentageText.text += PlayerPrefs.GetInt("OutOfBoundsCount", 0) + "\n\n";

            percentageText.text += Utils.FormatMillisecondsToHMS(grandTotalPlatinum) + "\n\n";
            percentageText.text += Utils.FormatTimeDHMS((double)PlayerPrefs.GetFloat("TotalRuntimeSeconds", 0f));
        }

        else if (PlayMissionManager.selectedGame == Game.gold)
        {
            int totalBeginnerGold = GetTotalCompletion(Game.gold, Type.beginner);
            int totalIntermediateGold = GetTotalCompletion(Game.gold, Type.intermediate);
            int totalAdvancedGold = GetTotalCompletion(Game.gold, Type.advanced);
            int total = totalBeginnerGold + totalIntermediateGold + totalAdvancedGold;
            int goldTimes = GetTotalPlatinumGold(Game.gold, Type.beginner) + GetTotalPlatinumGold(Game.gold, Type.intermediate) + GetTotalPlatinumGold(Game.gold, Type.advanced) + GetTotalPlatinumGold(Game.gold, Type.expert);
            long grandTotalGold = GetTotalTime(Game.gold);
            int totalEasterEgg = PlayerPrefs.GetInt("EasterEggCollected", 0);

            captions.text = "Beginner:\n" +
                "Intermediate:\n" +
                "Advanced:\n" +
                "Total:\n" +
                "\n" +
                "Gold times:\n" +
                "Out of Bounds:\n" +
                "Grand Total (hours:minutes:seconds):\n" +
                "\n" +
                "Total Waster Time (days:hours:minutes:seconds):\n";

            completionText.text = GetColor(Mathf.FloorToInt(goldTimes / 100)) + goldTimes + "/100</color>";

            int percentage = 0;

            percentage = Mathf.FloorToInt(totalBeginnerGold * 100 / 24);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt(totalIntermediateGold * 100 / 24);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt(totalAdvancedGold * 100 / 52);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentage = Mathf.FloorToInt((totalBeginnerGold + totalIntermediateGold + totalAdvancedGold) * 100 / 100);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";

            percentageText.text += "\n";

            percentage = Mathf.FloorToInt(goldTimes * 100 / 120);
            percentageText.text += GetColor(percentage) + percentage + "%</color>\n";
            percentageText.text += PlayerPrefs.GetInt("OutOfBoundsCount", 0) + "\n\n";

            percentageText.text += Utils.FormatMillisecondsToHMS(grandTotalGold) + "\n\n";
            percentageText.text += Utils.FormatTimeDHMS((double)PlayerPrefs.GetFloat("TotalRuntimeSeconds", 0f));
        }
    }

    public string GetColor(int percentage)
    {
        if (percentage >= 0 && percentage < 50)
            return "";
        else if (percentage >= 50 && percentage < 100)
            return "<color=#888C8D>";
        else
            return "<color=#FFDD00>";
    }

    public int GetTotalCompletion(Game game, Type type)
    {
        List<Mission> missionList = GetMissionList(game, type);

        int totalCompletion = 0;
        foreach (Mission m in missionList)
        {
            if (PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1) != -1)
                totalCompletion++;

        }
        return totalCompletion;
    }

    public int GetTotalPlatinumGold(Game game, Type type)
    {
        List<Mission> missionList = GetMissionList(game, type);

        int totalPlatinumGold = 0;
        foreach (Mission m in missionList)
        {
            float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
            if (time != -1 && time < m.goldTime)
                totalPlatinumGold++;

        }
        return totalPlatinumGold;
    }

    public int GetTotalUltimate(Game game, Type type)
    {
        List<Mission> missionList = GetMissionList(game, type);

        int totalUltimate = 0;
        foreach (Mission m in missionList)
        {
            float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
            if (time != -1 && time < m.ultimateTime)
                totalUltimate++;
        }
        return totalUltimate;
    }

    public long GetTotalTime(Game game)
    {
        List<Mission> missionList = new List<Mission>();
        long total = 0;
        if (game == Game.gold)
        {
            missionList = GetMissionList(Game.gold, Type.beginner);
            foreach (Mission m in missionList)
            {
                float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
                if (time != -1) total += (long)time;
            }

            missionList = GetMissionList(Game.gold, Type.intermediate);
            foreach (Mission m in missionList)
            {
                float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
                if (time != -1) total += (long)time;
            }

            missionList = GetMissionList(Game.gold, Type.advanced);
            foreach (Mission m in missionList)
            {
                float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
                if (time != -1) total += (long)time;
            }
        }
        else if (game == Game.platinum)
        {
            missionList = GetMissionList(Game.platinum, Type.beginner);
            foreach (Mission m in missionList)
            {
                float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
                if (time != -1) total += (long)time;
            }

            missionList = GetMissionList(Game.platinum, Type.intermediate);
            foreach (Mission m in missionList)
            {
                float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
                if (time != -1) total += (long)time;
            }

            missionList = GetMissionList(Game.platinum, Type.advanced);
            foreach (Mission m in missionList)
            {
                float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
                if (time != -1) total += (long)time;
            }

            missionList = GetMissionList(Game.platinum, Type.expert);
            foreach (Mission m in missionList)
            {
                float time = PlayerPrefs.GetFloat(m.levelName + "_Time_" + 0, -1);
                if (time != -1) total += (long)time;
            }
        }
        return total;
    }

    public List<Mission> GetMissionList(Game game, Type type)
    {
        List<Mission> missionList = new List<Mission>();
        if (game == Game.platinum)
        {
            if (type == Type.beginner)
                missionList = MissionInfo.instance.missionsPlatinumBeginner;
            if (type == Type.intermediate)
                missionList = MissionInfo.instance.missionsPlatinumIntermediate;
            if (type == Type.advanced)
                missionList = MissionInfo.instance.missionsPlatinumAdvanced;
            if (type == Type.expert)
                missionList = MissionInfo.instance.missionsPlatinumExpert;
        }
        else if (game == Game.gold)
        {
            if (type == Type.beginner)
                missionList = MissionInfo.instance.missionsGoldBeginner;
            if (type == Type.intermediate)
                missionList = MissionInfo.instance.missionsGoldIntermediate;
            if (type == Type.advanced)
                missionList = MissionInfo.instance.missionsGoldAdvanced;
            if (type == Type.custom)
                missionList = MissionInfo.instance.missionsGoldCustom;
        }

        return missionList;
    }
}
