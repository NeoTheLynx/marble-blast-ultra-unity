using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TS;
using System.Text.RegularExpressions;

public class MissionInfo : MonoBehaviour
{
    public static MissionInfo instance;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [HideInInspector] public string highScoreName;

    [Header("Selected Mission Directory")]
    [TextArea(1, 2)] public string MissionPath;
    public string missionName;

    [Header("Selected Mission Info")]
    public int time;
    public string levelName;
    [TextArea(2, 10)] public string description;
    [TextArea(2, 10)] public string startHelpText;
    public int level;
    public string artist;
    public int goldTime;
    public int ultimateTime;
    public int alarmTime;
    public string music;
    public string skybox;
    public bool hasEgg;

    [Header("Load Mission")]
    public List<Mission> missionsPlatinumBeginner = new List<Mission>();
    public List<Mission> missionsPlatinumIntermediate = new List<Mission>();
    public List<Mission> missionsPlatinumAdvanced = new List<Mission>();
    public List<Mission> missionsPlatinumExpert = new List<Mission>();
    public List<Mission> missionsGoldBeginner = new List<Mission>();
    public List<Mission> missionsGoldIntermediate = new List<Mission>();
    public List<Mission> missionsGoldAdvanced = new List<Mission>();
    public List<Mission> missionsGoldCustom = new List<Mission>();

    List<TSObject> MissionObjects;

    public void Start()
    {
        highScoreName = PlayerPrefs.GetString("HighScoreName", "");

        LoadMissions(Type.beginner, Game.platinum);
        LoadMissions(Type.intermediate, Game.platinum);
        LoadMissions(Type.advanced, Game.platinum);
        LoadMissions(Type.expert, Game.platinum);
        LoadMissions(Type.beginner, Game.gold);
        LoadMissions(Type.intermediate, Game.gold);
        LoadMissions(Type.advanced, Game.gold);
        LoadMissions(Type.custom, Game.gold);
    }

    public void LoadMissions(Type difficulty, Game game)
    {
        string path = string.Empty;
        if (difficulty == Type.custom)
            path = "marble/data/missions/";
        else if (game == Game.platinum)
            path = "marble/data/missions_mbp/";
        else if(game == Game.gold)
            path = "marble/data/missions_mbg/";

        string basePath = Path.Combine(
            Application.streamingAssetsPath,
            path,
            difficulty.ToString()
        );

        // 🔒 SAFETY 1: folder does not exist
        if (!Directory.Exists(basePath))
            return;

        string[] misFiles = Directory.GetFiles(basePath, "*.mis");

        // 🔒 SAFETY 2: folder exists but no missions
        if (misFiles == null || misFiles.Length == 0)
            return;

        foreach (string misPath in misFiles)
        {
            string levelName = Path.GetFileNameWithoutExtension(misPath);

            // Try jpg first, then png
            string jpgPath = Path.Combine(basePath, levelName + ".jpg");
            string pngPath = Path.Combine(basePath, levelName + ".png");

            string imagePath = null;

            if (File.Exists(jpgPath))
                imagePath = jpgPath;
            else if (File.Exists(pngPath))
                imagePath = pngPath;

            Sprite sprite;
            if (!string.IsNullOrEmpty(imagePath))
            {
                byte[] imageData = File.ReadAllBytes(imagePath);

                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.LoadImage(imageData);

                sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f),
                    100f
                );
            }
            else
            {
                sprite = null;
            }


            Mission newMission = new Mission
            {
                levelImage = sprite,
                directory = path + difficulty.ToString() + "/" + levelName + ".mis",
                levelNumber = -1,
                hasEgg = false
            };

            var lexer = new TSLexer(
                new AntlrFileStream(Path.Combine(Application.streamingAssetsPath, misPath))
            );
            var parser = new TSParser(new CommonTokenStream(lexer));
            var file = parser.start();

            if (parser.NumberOfSyntaxErrors > 0)
            {
                Debug.LogError("Could not parse mission file");
                continue;
            }

            MissionObjects = new List<TSObject>();

            foreach (var decl in file.decl())
            {
                var objectDecl = decl.stmt()?.expression_stmt()?.stmt_expr()?.object_decl();
                if (objectDecl == null)
                    continue;

                MissionObjects.Add(MissionImporter.ProcessObject(objectDecl));
            }

            if (MissionObjects.Count == 0)
                return;

            var mission = MissionObjects[0];
            foreach (var obj in mission.GetFirstChildrens())
            {
                //Mission info
                if (obj.ClassName == "ScriptObject" && obj.Name == "MissionInfo")
                {
                    int _time = -1;
                    if (int.TryParse(obj.GetField("time"), out _time))
                        if (_time != 0)
                            newMission.time = _time;
                        else
                            newMission.time = -1;
                    else
                        newMission.time = -1;

                    newMission.missionName = Regex.Unescape(levelName);
                    newMission.levelName = Regex.Unescape(obj.GetField("name"));
                    newMission.description = Regex.Unescape(obj.GetField("desc"));

                    string startHelpText = obj.GetField("startHelpText");
                    if (!string.IsNullOrEmpty(startHelpText))
                        newMission.startHelpText = Regex.Unescape(startHelpText);

                    int _level = 0;
                    if (int.TryParse(obj.GetField("level"), out _level))
                        newMission.levelNumber = _level;
                    else
                        newMission.levelNumber = 0;

                    newMission.artist = (obj.GetField("artist"));

                    int _goldTime = -1;
                    if (int.TryParse(obj.GetField("goldTime"), out _goldTime))
                        newMission.goldTime = _goldTime;
                    else
                        newMission.goldTime = -1;

                    int _ultimateTime = -1;
                    if (int.TryParse(obj.GetField("ultimateTime"), out _ultimateTime))
                        newMission.ultimateTime = _ultimateTime;
                    else
                        newMission.ultimateTime = -1;

                    int _alarmTime = 15;
                    if (int.TryParse(obj.GetField("AlarmStartTime"), out _alarmTime))
                        newMission.alarmTime = _alarmTime;
                    else
                        newMission.alarmTime = 15;

                    newMission.music = obj.GetField("music");
                }
                else if (obj.ClassName == "Sky")
                {
                    var skybox = ResolvePath(obj.GetField("materialList"), MissionInfo.instance.MissionPath);

                    newMission.skyboxName = Path.GetFileNameWithoutExtension(skybox);
                }
                else if(obj.ClassName == "Item" && obj.GetField("dataBlock") == "EasterEgg")
                {
                    newMission.hasEgg = true;
                }
            }

            if(newMission.levelImage)
                newMission.levelImage.name = levelName;

            if(game == Game.gold)
            {
                if (difficulty == Type.beginner)
                    missionsGoldBeginner.Add(newMission);
                else if (difficulty == Type.intermediate)
                    missionsGoldIntermediate.Add(newMission);
                else if (difficulty == Type.advanced)
                    missionsGoldAdvanced.Add(newMission);
                else if (difficulty == Type.custom)
                    missionsGoldCustom.Add(newMission);
            }
            else if(game == Game.platinum)
            {
                if (difficulty == Type.beginner)
                    missionsPlatinumBeginner.Add(newMission);
                else if (difficulty == Type.intermediate)
                    missionsPlatinumIntermediate.Add(newMission);
                else if (difficulty == Type.advanced)
                    missionsPlatinumAdvanced.Add(newMission);
                else if (difficulty == Type.expert)
                    missionsPlatinumExpert.Add(newMission);
            }
            
        }

        if (game == Game.gold)
        {
            if (difficulty == Type.beginner)
                missionsGoldBeginner = SortMissionsByLevelNumber(missionsGoldBeginner);
            else if (difficulty == Type.intermediate)
                missionsGoldIntermediate = SortMissionsByLevelNumber(missionsGoldIntermediate);
            else if (difficulty == Type.advanced)
                missionsGoldAdvanced = SortMissionsByLevelNumber(missionsGoldAdvanced);
            else if (difficulty == Type.custom)
                missionsGoldCustom = SortMissionsByLevelNumber(missionsGoldCustom);
        }
        else if (game == Game.platinum)
        {
            if (difficulty == Type.beginner)
                missionsPlatinumBeginner = SortMissionsByLevelNumber(missionsPlatinumBeginner);
            else if (difficulty == Type.intermediate)
                missionsPlatinumIntermediate = SortMissionsByLevelNumber(missionsPlatinumIntermediate);
            else if (difficulty == Type.advanced)
                missionsPlatinumAdvanced = SortMissionsByLevelNumber(missionsPlatinumAdvanced);
            else if (difficulty == Type.expert)
                missionsPlatinumExpert = SortMissionsByLevelNumber(missionsPlatinumExpert);
        }
    }

    public static List<Mission> SortMissionsByLevelNumber(List<Mission> missions)
    {
        if (missions == null || missions.Count == 0)
            return new List<Mission>();

        int maxIndex = missions.Count - 1;

        var sortable = missions
            .Where(m => m != null)
            .Select(m => new
            {
                Mission = m,
                NormalizedLevel = Mathf.Clamp(m.levelNumber, 0, (maxIndex + 1))
            })
            .ToList();

        // 🔥 IMPORTANT PART:
        // levelNumber ASC
        // levelName DESC (so later alphabet inserts first)
        sortable.Sort((a, b) =>
        {
            int numCompare = a.NormalizedLevel.CompareTo(b.NormalizedLevel);
            if (numCompare != 0)
                return numCompare;

            return string.Compare(
                b.Mission.levelName,
                a.Mission.levelName,
                StringComparison.OrdinalIgnoreCase
            );
        });

        List<Mission> result = new List<Mission>();

        foreach (var entry in sortable)
        {
            int targetIndex = Mathf.Clamp(
                entry.NormalizedLevel,
                0,
                result.Count
            );

            result.Insert(targetIndex, entry.Mission);
        }

        for (int i = 0; i < result.Count; i++)
            result[i].levelNumber = (i + 1);

        return result;
    }

    private string ResolvePath(string assetPath, string misPath)
    {
        if (string.IsNullOrEmpty(assetPath))
            return assetPath;

        // Normalize slashes first
        assetPath = assetPath.Replace('\\', '/');

        // Remove trailing quote if present
        if (assetPath.EndsWith("\""))
            assetPath = assetPath.Substring(0, assetPath.Length - 1);

        // Remove leading slashes
        assetPath = assetPath.TrimStart('/');

        // --- Resolve special prefixes ---
        if (assetPath[0] == '.')
        {
            // Relative to mission file
            assetPath = Path.GetDirectoryName(misPath).Replace('\\', '/')
                        + assetPath.Substring(1);
        }
        else
        {
            // Replace anything before first '/' with "marble"
            int slash = assetPath.IndexOf('/');
            assetPath = slash >= 0
                ? "marble" + assetPath.Substring(slash)
                : "marble/" + assetPath;
        }

        // --- Fix lbinteriors_* folders ---
        assetPath = assetPath
            .Replace("/lbinteriors_mbp/", "/interiors_mbp/")
            .Replace("/lbinteriors_mbg/", "/interiors_mbg/");

        return assetPath;
    }
}
