using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public void Awake()
    {
        instance = this;

        InitializeOutOfBoundsInsult();

        onFinish.AddListener(Finish);
        onOutOfBounds.AddListener(OutOfBounds);
        onCollectGem.AddListener(UpdateGem);

        Marble.onRespawn.AddListener(Respawn);

        StartCoroutine(AssignReferences());
    }

    public GameObject mainCam;
    public GameObject gameUIManager;

    IEnumerator AssignReferences()
    {
        while (!Marble.instance)
        {
            yield return null;
        }

        startPad = GameObject.Find("StartPad");
        finishPad = GameObject.Find("EndPad");

        mainCam.SetActive(true);
        gameUIManager.SetActive(true);

        activeCheckpoint = startPad.transform.Find("Spawn");
        activeCheckpointGravityDir = Vector3.down;
    }

    [HideInInspector] public GameObject startPad;
    [HideInInspector] public GameObject finishPad;

    [Space]
    [Header("Audio Clips")]
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip puSpawn;
    [SerializeField] AudioClip puReady;
    [SerializeField] AudioClip puSet;
    [SerializeField] AudioClip puGo;
    [SerializeField] AudioClip puFinish;
    [SerializeField] AudioClip puOutOfBounds;
    [SerializeField] AudioClip puHelp;
    [SerializeField] AudioClip puMissingGems;
    [SerializeField] AudioClip checkpointSfx;
    [SerializeField] AudioClip overParTimeSfx;

    public void PlayJumpAudio() => audioSource.PlayOneShot(jump);
    public void PlaySpawnAudio() => audioSource.PlayOneShot(puSpawn);
    public void PlayReadyAudio() => audioSource.PlayOneShot(puReady);
    public void PlaySetAudio() => audioSource.PlayOneShot(puSet);
    public void PlayGoAudio() => audioSource.PlayOneShot(puGo);
    public void PlayFinishAudio() => audioSource.PlayOneShot(puFinish);
    public void PlayOutOfBoundsAudio() => audioSource.PlayOneShot(puOutOfBounds);
    public void PlayHelpAudio() => audioSource.PlayOneShot(puHelp);
    public void PlayMissingGemAudio() => audioSource.PlayOneShot(puMissingGems);
    public void PlayAudioClip(AudioClip _ac) => audioSource.PlayOneShot(_ac);


    public void PlayLevelMusic()
    {
        JukeboxManager.instance.audioSource.volume = PlayerPrefs.GetFloat("Audio_MusicVolume", 0.5f);
        //JukeboxManager.instance.PlayMusic(MissionInfo.instance.music);
    }

    public void SetSoundVolumes()
    {
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
            audioSource.volume = PlayerPrefs.GetFloat("Audio_SoundVolume", 0.5f);
    }

    [Space]
    [Header("UI Menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject finishMenu;
    [SerializeField] GameObject enterNameMenu;
    [SerializeField] TextMeshProUGUI finalTime;
    [SerializeField] TextMeshProUGUI finishCaption;
    [SerializeField] TextMeshProUGUI namesCaption;
    [SerializeField] TextMeshProUGUI timesCaption;
    [SerializeField] TextMeshProUGUI enterNameCaption;
    [SerializeField] GameObject platinumTimeBox;
    [SerializeField] GameObject ultimateTimeBox;
    [SerializeField] GameObject goldTimeBox;
    [SerializeField] TextMeshProUGUI parTimeText;
    [SerializeField] TextMeshProUGUI timePassedText;
    [SerializeField] TextMeshProUGUI clockBonusesText;
    [SerializeField] Button replayButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button noButton;
    [SerializeField] Button yesButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button okayButton;
    [SerializeField] TMP_InputField nameInputField;

    public Transform activeCheckpoint;
    public Vector3 activeCheckpointGravityDir;
    [HideInInspector] public List<GameObject> recentGems = new List<GameObject>();
    [HideInInspector] public PowerupType tempPowerup;
    bool useCheckpoint;

    [Space]
    [SerializeField] AudioSource audioSource;

    bool startTimer;
    [HideInInspector] public bool timeTravelActive;
    [HideInInspector] public float elapsedTime;
    float bonusTime;
    string bestTimeName = string.Empty;

    int totalGems;
    [HideInInspector] public int currentGems;
    Gem[] gems;

    [HideInInspector] public PowerupType activePowerup;
    [HideInInspector] public bool superBounceIsActive = false;
    [HideInInspector] public bool shockAbsorberIsActive = false;
    [HideInInspector] public bool gyrocopterIsActive = false;
    [HideInInspector] public float sbsaActiveTime;
    [HideInInspector] public float gyroActiveTime;
    [HideInInspector] public float timeTravelStartTime;
    [HideInInspector] public float timeTravelBonus;

    [Header("Particles")]
    public GameObject finishParticles;
    GameObject finishParticleInstance;

    //game state
    [Space]
    public static bool gameFinish = false;
    public static bool gameStart = false;
    public static bool isPaused = false;
    public static bool alarmIsPlaying = false;
    public static bool notQualified = false;
    [HideInInspector] public bool alarmCoroutineStarted = false;

    //events
    public class OnFinish : UnityEvent { };
    public static OnFinish onFinish = new OnFinish();
    public class OnOutOfBounds : UnityEvent { };
    public static OnOutOfBounds onOutOfBounds = new OnOutOfBounds();
    public class OnCollectGem : UnityEvent<int> { };
    public static OnCollectGem onCollectGem = new OnCollectGem();
    public class OnReachCheckpoint : UnityEvent<Transform, Vector3> { };
    public static OnReachCheckpoint onReachCheckpoint = new OnReachCheckpoint();

    Coroutine alarmCoroutine;

    void Start()
    {
        isPaused = false;

        startTimer = false;
        timeTravelActive = false;
        activePowerup = PowerupType.None;

        //disable UI
        finishMenu.SetActive(false);
        pauseMenu.SetActive(false);

        okayButton.onClick.AddListener(CloseEnterNameWindow);
        replayButton.onClick.AddListener(ReplayLevel);
        continueButton.onClick.AddListener(ReturnToMenu);
        noButton.onClick.AddListener(TogglePause);
        yesButton.onClick.AddListener(ReturnToMenu);
        restartButton.onClick.AddListener(RestartLevel);

        nameInputField.onEndEdit.AddListener(UpdateName);

        UpdateBestTimes();

        spawnAudioPlayed = false;

        onReachCheckpoint.AddListener(ReachCheckpoint);
        useCheckpoint = false;

        gameStart = false;
        gameFinish = false;
        alarmCoroutineStarted = false;
    }

    public void InitGemCount()
    {
        gems = FindObjectsOfType<Gem>();

        totalGems = gems.Length;
        if (totalGems != 0) {
            GameUIManager.instance.SetTargetGem(totalGems);
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Gem");
            foreach(GameObject obst in obstacles) {
                obst.active = false;
            }
        }
        else {
            GameUIManager.instance.ShowGemCountUI(false);
        }
    }

    #region Game
    public PowerupType ConsumePowerup()
    {
        PowerupType powerup = activePowerup;
        activePowerup = PowerupType.None;

        GameUIManager.instance.SetPowerupIcon(activePowerup);

        return powerup;
    }

    private void Update()
    {
        if (activeCheckpoint == null) return;

        //Handle Timer
        if (startTimer && !timeTravelActive)
        {
            elapsedTime += Time.deltaTime * 1000f;
            elapsedTime = Mathf.RoundToInt(elapsedTime);
            GameUIManager.instance.SetTimerText(elapsedTime);
        }
        else if (timeTravelActive)
        {
            GameUIManager.instance.SetTimerText(elapsedTime);
        }

        if (gameStart && MissionInfo.instance.time != -1 && elapsedTime >= (MissionInfo.instance.time - MissionInfo.instance.alarmTime * 1000))
        {
            if (elapsedTime >= MissionInfo.instance.time)
            {
                alarmIsPlaying = false;
                notQualified = true;
            }
            else
            {
                alarmIsPlaying = true;
                notQualified = false;

                if (!alarmCoroutineStarted)
                {
                    alarmCoroutineStarted = true;
                    alarmCoroutine = StartCoroutine(AlarmCoroutine());
                }
            }
        }
        else
        {
            notQualified = false;
        }

        //Handle Shock Absorber and Super Bounce timer
        if (shockAbsorberIsActive || superBounceIsActive)
        {
            if (Time.time - sbsaActiveTime > 5f)
                Marble.instance.RevertMaterial();
        }

        //Handle Gyrocopter Timer
        if (gyrocopterIsActive)
        {
            if (Time.time - gyroActiveTime > 5f)
                Marble.instance.CancelGyrocopter();
        }

        //Handle Time travel timer
        if (timeTravelActive)
        {
            bonusTime += Time.deltaTime * 1000f;

            float elapsed = Time.time - timeTravelStartTime;

            if (elapsed >= timeTravelBonus)
            {
                float overshoot = elapsed - timeTravelBonus;

                // bonusTime is scaled by *1000f, so convert overshoot the same way
                bonusTime -= overshoot * 1000f;

                Marble.instance.InactivateTimeTravel();
            }
        }

        //pause
        if (Input.GetKeyDown(KeyCode.Escape) && !gameFinish)
            TogglePause();

        if (gameFinish)
        {
            if (enterNameMenu.activeSelf && Input.GetKeyDown(KeyCode.Return))
                CloseEnterNameWindow();
            else if (finishMenu.activeSelf && Input.GetKeyDown(KeyCode.Return))
                ReturnToMenu();
        }
    }

    public IEnumerator AlarmCoroutine()
    {
        GameUIManager.instance.SetCenterText(
            "You have " + (MissionInfo.instance.alarmTime) + " seconds remaining."
        );

        Marble.instance.alarmSound.Play();

        float time = 0f;

        while (!notQualified)
        {
            if (!timeTravelActive)
                time += Time.deltaTime;

            int seconds = Mathf.FloorToInt(time);
            GameUIManager.instance.SetTimerColor(seconds % 2 == 0);

            yield return null; // wait exactly one frame
        }

        GameUIManager.instance.SetCenterText("The clock has passed the Par Time - please retry the level.");
        Marble.instance.alarmSound.Stop();
        PlayAudioClip(overParTimeSfx);
    }

    void DisablePreviews(){
        Preview.instance.setIsInPreviewMode(false);
    }
    void EnablePreviews(){
        Preview.instance.setIsInPreviewMode(true);
    }


    public void TogglePause()
    {
        if (GameUIManager.instance.isInitialized && GameUIManager.instance.oobInsultMenu.activeSelf)
            return;

        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool CheckForAllGems() => (totalGems == currentGems);

    void UpdateGem(int _count)
    {
        if (totalGems == 0) return;

        //negative symbol means no center text message
        currentGems = Mathf.Abs(_count);

        GameUIManager.instance.SetCurrentGem(currentGems);

        string remainingGemMsg;

        if (currentGems + 1 == totalGems) remainingGemMsg = "You picked up a diamond! Only one more diamond to go!";
        else if (currentGems == totalGems) remainingGemMsg = "You picked up all diamonds! Head for the finish!";
        else remainingGemMsg = "You picked up a diamond! " + (totalGems - currentGems) + " diamonds to go!";

        if (_count > 0)
            GameUIManager.instance.SetBottomText(remainingGemMsg);
    }

    void OutOfBounds()
    {
        IncrementOutOfBoundsCount();

        GameUIManager.instance.SetCenterImage(3);
        PlayOutOfBoundsAudio();
        CameraController.instance.LockCamera(false);

        CancelInvoke();
        Invoke(nameof(InvokeRespawn), 2f);
    }

    public void IncrementOutOfBoundsCount()
    {
        int oobCount = PlayerPrefs.GetInt("OutOfBoundsCount", 0);
        oobCount++;
        PlayerPrefs.SetInt("OutOfBoundsCount", oobCount);

        for (int i = 0; i < specialThresholds.Length; i++)
        {
            if (oobCount == specialThresholds[i])
            {
                GameUIManager.instance.SetOutOfBoundsMessage(oobCount, oobSpecial[i]);
                return;
            }
        }

        if (oobCount != 0 && oobCount % 200 == 0)
        {
            int index = Random.Range(0, oobRandom.Length);
            GameUIManager.instance.SetOutOfBoundsMessage(oobCount, oobRandom[index]);
            return;
        }
    }

    public void InvokeRespawn() => Marble.onRespawn?.Invoke();

    public IEnumerator ResetSpawnAudio()
    {
        yield return new WaitForSeconds(0.1f);
        spawnAudioPlayed = false;
    }
    public void RestartLevel()
    {
        TogglePause();

        activeCheckpoint = startPad.transform.Find("Spawn");
        activeCheckpointGravityDir = Vector3.down;
        useCheckpoint = false;

        Marble.onRespawn?.Invoke();
    }

    public void ReplayLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        finishMenu.SetActive(false);
        Marble.onRespawn?.Invoke();
    }

    public void ReachCheckpoint(Transform checkpoint, Vector3 checkpointGravityDir)
    {
        if (checkpoint == activeCheckpoint) return;

        useCheckpoint = true;

        GameUIManager.instance.SetBottomText("Checkpoint reached!");
        activeCheckpoint = checkpoint;
        tempPowerup = activePowerup;

        activeCheckpointGravityDir = checkpointGravityDir;

        PlayAudioClip(checkpointSfx);

        recentGems.Clear();
    }

    bool spawnAudioPlayed = false;
    public void Respawn()
    {
        if (!spawnAudioPlayed)
        {
            PlaySpawnAudio();
            spawnAudioPlayed = true;

            StartCoroutine(ResetSpawnAudio());
        }

        CancelInvoke();
        gameFinish = false;
        Movement.instance.freezeMovement = false;

        GravityModifier.ResetGravityGlobal();
        CameraController.instance?.ResetCam();

        CameraController.instance.LockCamera(true);

        if (!useCheckpoint)
        {
            Movement.instance.StopAllMovement();
            Movement.instance.StopAllbutJumping();

            alarmIsPlaying = false;
            GameUIManager.instance.SetTimerText(0);

            alarmCoroutineStarted = false;

            if (alarmCoroutine != null)
                StopCoroutine(alarmCoroutine);

            Marble.instance.alarmSound.Stop();

            GameStateStart();
        }
        else
        {
            Movement.instance.StopAllMovement();
            Movement.instance.StartMoving();

            foreach (GameObject gem in recentGems)
            {
                gem.SetActive(true);
                currentGems--;
            }

            GameUIManager.instance.SetCurrentGem(currentGems);

            activePowerup = tempPowerup;
            GameUIManager.instance.SetPowerupIcon(activePowerup);

            GameUIManager.instance.SetCenterImage(-1);
        }

        recentGems.Clear();
        Marble.instance.RevertMaterial();
        Marble.instance.ToggleGyrocopterBlades(false);
        if (gyrocopterIsActive)
            Marble.instance.CancelGyrocopter();
        Marble.instance.InactivateTimeTravel();
    }


    void GameStateStart()
    {
        gameStart = false;

        startTimer = false;
        UpdateGem(0);
        elapsedTime = bonusTime = 0;

        foreach (Gem gem in gems)
            gem.gameObject.SetActive(true);

        ConsumePowerup();

        //reset powerups
        foreach (Powerups po in FindObjectsOfType<Powerups>())
            if (po.powerupType != PowerupType.EasterEgg)
                po.Activate(false);

        //reset moving platforms
        foreach (MovingPlatform mp in FindObjectsOfType<MovingPlatform>())
            mp.ResetMP();

        GameUIManager.instance.SetTimerText(elapsedTime);

        string startHelpText = MissionInfo.instance.startHelpText;
        if (!string.IsNullOrEmpty(startHelpText))
            GameUIManager.instance.SetCenterText(startHelpText);

        if (finishParticleInstance)
            Destroy(finishParticleInstance);

        GameUIManager.instance.SetCenterImage(-1);
        Invoke(nameof(GameStateReady), 0.5f);
    }

    void GameStateReady()
    {
        PlayReadyAudio();
        GameUIManager.instance.SetCenterImage(0);
        Invoke(nameof(GameStateSet), 1.5f);
    }
    void GameStateSet()
    {
        PlaySetAudio();
        GameUIManager.instance.SetCenterImage(1);
        Invoke(nameof(GameStateGo), 1.5f);
    }

    void GameStateGo()
    {
        PlayGoAudio();

        startTimer = true;
        gameStart = true;

        GameUIManager.instance.SetCenterImage(2);
        Movement.instance.StartMoving();
        Invoke(nameof(ClearCenterImage), 2f);
    }

    void ClearCenterImage()
    {
        GameUIManager.instance.SetCenterImage(-1);
    }

    void Finish()
    {
        //Missing gems
        if (totalGems != 0 && totalGems != currentGems)
        {
            GameUIManager.instance.SetBottomText("You can't finish without all diamonds!");

            PlayMissingGemAudio();
        }
        //Finish
        else
        {
            CancelInvoke();
            PlayFinishAudio();

            startTimer = false;
            GameUIManager.instance.SetBottomText("Congratulations! You've finished!");

            finishParticleInstance = Instantiate(finishParticles, finishPad.transform.Find("FinishParticle").position, Quaternion.identity);
            finishParticleInstance.transform.localScale = Vector3.one * 1.5f;
            finishParticleInstance.transform.rotation = finishPad.transform.rotation;

            Marble.instance.InactivateTimeTravel();

            gameFinish = true;
            GameUIManager.instance.SetTimerText(elapsedTime);

            CameraController.onCameraFinish?.Invoke();
            Invoke(nameof(StopMarbleMovement), 0.0625f);
            Invoke(nameof(ShowFinishUI), 2f);
        }

    }
    void StopMarbleMovement()
    {
        Movement.instance.freezeMovement = true;
        Movement.instance.StopMoving();
    }
    #endregion

    #region UI
    public void ReturnToMenu()
    {
        //JukeboxManager.instance.PlayMusic("Pianoforte");
        EnablePreviews();
        SceneManager.LoadScene("PlayMission");
    }

    public void ShowFinishUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        finishMenu.SetActive(true);
        GenerateFinishUIText();
    }

    public void UpdateName(string s)
    {
        bestTimeName = s;
        PlayerPrefs.SetString("HighScoreName", s);
        MissionInfo.instance.highScoreName = s;
    }

    public void CloseEnterNameWindow()
    {
        enterNameMenu.SetActive(false);
        replayButton.interactable = true;
        continueButton.interactable = true;

        InsertBestTime(bestTimeName, elapsedTime);
        UpdateBestTimes();
    }

    public void GenerateFinishUIText()
    {
        replayButton.interactable = true;
        continueButton.interactable = true;

        bool gold = elapsedTime < MissionInfo.instance.goldTime;
        bool ultimate = elapsedTime < MissionInfo.instance.ultimateTime;
        bool qualify = !(MissionInfo.instance.time != -1 && elapsedTime >= MissionInfo.instance.time);
        finalTime.text = Utils.FormatTime(elapsedTime);

        int pos = DeterminePosition(elapsedTime);
        if (pos != -1 && qualify)
        {
            replayButton.interactable = false;
            continueButton.interactable = false;
            enterNameMenu.SetActive(true);
            if (pos == 0)
                enterNameCaption.text = "You got the top time!";
            else if (pos == 1)
                enterNameCaption.text = "You got the second top time!";
            else if (pos == 2)
                enterNameCaption.text = "You got the third top time!";

            nameInputField.SetTextWithoutNotify(MissionInfo.instance.highScoreName);
            UpdateName(MissionInfo.instance.highScoreName);
        }

        string _qualifyTime, _goldTime, _platinumTime, _ultimateTime;
        if (!qualify)
            _qualifyTime = "<color=#F55555>" + Utils.FormatTime(MissionInfo.instance.time) + "</color>";
        else
            _qualifyTime = Utils.FormatTime(MissionInfo.instance.time);

        parTimeText.text = _qualifyTime;

        _goldTime = "<color=#FFEE11>" + Utils.FormatTime(MissionInfo.instance.goldTime) + "</color>";
        _platinumTime = "<color=#CCCCCC>" + Utils.FormatTime(MissionInfo.instance.goldTime) + "</color>";
        _ultimateTime = "<color=#FFCC33>" + Utils.FormatTime(MissionInfo.instance.ultimateTime) + "</color>";

        goldTimeBox.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _goldTime;
        platinumTimeBox.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _platinumTime;
        ultimateTimeBox.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _ultimateTime;

        timePassedText.text = Utils.FormatTime(elapsedTime + bonusTime);
        clockBonusesText.text = Utils.FormatTime(bonusTime);

        platinumTimeBox.SetActive(false);
        ultimateTimeBox.SetActive(false);
        goldTimeBox.SetActive(false);

        if (PlayMissionManager.selectedGame == Game.gold)
        {
            if (PlayMissionManager.currentlySelectedType == Type.custom)
            {
                platinumTimeBox.SetActive(true);
                ultimateTimeBox.SetActive(true);

                if (ultimate && qualify)
                    finishCaption.text = "You beat the <color=#FFCC33>Ultimate</color> Time!";
                else if (gold && qualify)
                    finishCaption.text = "You beat the <color=#CCCCCC>Platinum</color> Time!";
                else if (qualify)
                    finishCaption.text = "You beat the Par Time";
                else
                    finishCaption.text = "<color=#F55555>You did't pass the Par Time!</color>";
            }
            else
            {
                goldTimeBox.SetActive(true);

                if (gold && qualify)
                    finishCaption.text = "You beat the <color=#FFEE11>Gold</color> Time!";
                else if (qualify)
                    finishCaption.text = "You beat the Par Time";
                else
                    finishCaption.text = "<color=#F55555>You did't pass the Par time!</color>";
            }
        }
        else if (PlayMissionManager.selectedGame == Game.platinum)
        {
            platinumTimeBox.SetActive(true);
            ultimateTimeBox.SetActive(true);

            if (ultimate && qualify)
                finishCaption.text = "You beat the <color=#FFCC33>Ultimate</color> Time!";
            else if (gold && qualify)
                finishCaption.text = "You beat the <color=#CCCCCC>Platinum</color> Time!";
            else if (qualify)
                finishCaption.text = "You beat the Par Time";
            else
                finishCaption.text = "<color=#F55555>You did't pass the Par Time!</color>";
        }

        UpdateBestTimes();

        int qualifiedLevel = PlayerPrefs.GetInt("QualifiedLevel" + PlayMissionManager.CapitalizeFirst(PlayMissionManager.currentlySelectedType.ToString()) + PlayMissionManager.CapitalizeFirst(PlayMissionManager.selectedGame.ToString()), 0);
        if (qualify && qualifiedLevel + 1 == MissionInfo.instance.level)
            PlayerPrefs.SetInt("QualifiedLevel" + PlayMissionManager.CapitalizeFirst(PlayMissionManager.currentlySelectedType.ToString()) + PlayMissionManager.CapitalizeFirst(PlayMissionManager.selectedGame.ToString()), (qualifiedLevel + 1));

        PlayerPrefs.SetInt("SelectedLevel" + PlayMissionManager.CapitalizeFirst(PlayMissionManager.currentlySelectedType.ToString()) + PlayMissionManager.CapitalizeFirst(PlayMissionManager.selectedGame.ToString()), (MissionInfo.instance.level));
    }

    void UpdateBestTimes()
    {
        namesCaption.text = string.Empty;
        timesCaption.text = string.Empty;

        for (int i = 0; i < 3; i++)
        {
            string _name = PlayerPrefs.GetString(MissionInfo.instance.levelName + "_Name_" + i, "Matan W.");
            float _time = PlayerPrefs.GetFloat(MissionInfo.instance.levelName + "_Time_" + i, -1);
            namesCaption.text += _name + "\n";

            bool ultimate = _time < MissionInfo.instance.ultimateTime;
            bool gold = _time < MissionInfo.instance.goldTime;

            if (PlayMissionManager.selectedGame == Game.gold)
            {
                if (PlayMissionManager.currentlySelectedType == Type.custom)
                {
                    platinumTimeBox.SetActive(true);
                    ultimateTimeBox.SetActive(true);

                    if (_time != -1 && ultimate)
                        timesCaption.text += "<color=#FFCC33>" + Utils.FormatTime(_time) + "</color>" + "\n";
                    else if (_time != -1 && gold)
                        timesCaption.text += "<color=#CCCCCC>" + Utils.FormatTime(_time) + "</color>" + "\n";
                    else
                        timesCaption.text += Utils.FormatTime(_time) + "\n";
                }
                else
                {
                    if (_time != -1 && gold)
                        timesCaption.text += "<color=#FFEE11>" + Utils.FormatTime(_time) + "</color>" + "\n";
                    else
                        timesCaption.text += Utils.FormatTime(_time) + "\n";
                }
            }
            else if (PlayMissionManager.selectedGame == Game.platinum)
            {
                if (_time != -1 && ultimate)
                    timesCaption.text += "<color=#FFCC33>" + Utils.FormatTime(_time) + "</color>" + "\n";
                else if (_time != -1 && gold)
                    timesCaption.text += "<color=#CCCCCC>" + Utils.FormatTime(_time) + "</color>" + "\n";
                else
                    timesCaption.text += Utils.FormatTime(_time) + "\n";
            }
        }
    }

    int DeterminePosition(float time)
    {
        float[] times = new float[3];
        for (int i = 0; i < 3; i++)
            times[i] = PlayerPrefs.GetFloat(MissionInfo.instance.levelName + "_Time_" + i, -1);

        if (times[0] == -1 || time < times[0]) return 0;
        else if (times[1] == -1 || (time < times[1] && time >= times[0])) return 1;
        else if (times[2] == -1 || (time < times[2] && time >= times[1])) return 2;
        else return -1;
    }

    void InsertBestTime(string _name, float _time)
    {
        int pos = DeterminePosition(_time);
        if (pos == -1) return;

        for (int i = 1; i >= pos; i--)
        {
            string playerName = PlayerPrefs.GetString(MissionInfo.instance.levelName + "_Name_" + i, "Matan W.");
            float playerTime = PlayerPrefs.GetFloat(MissionInfo.instance.levelName + "_Time_" + i, -1);
            PlayerPrefs.SetString(MissionInfo.instance.levelName + "_Name_" + (i + 1), playerName);
            PlayerPrefs.SetFloat(MissionInfo.instance.levelName + "_Time_" + (i + 1), playerTime);
        }

        PlayerPrefs.SetString(MissionInfo.instance.levelName + "_Name_" + pos, _name);
        PlayerPrefs.SetFloat(MissionInfo.instance.levelName + "_Time_" + pos, _time);
    }
    #endregion

    string[] oobRandom;
    string[] oobSpecial;
    int[] specialThresholds;
    void InitializeOutOfBoundsInsult()
    {
        specialThresholds = new int[12]
        {
                1250, 2500, 3750, 5000, 6250, 7500, 8750,
            10000, 50000, 300000, 1000000, 30000000
        };

        oobRandom = new string[50]
        {
            "Let's be clear of the blatant truth: You suck!",
            "Honestly, do you have any control over the marble? It seems to have a life on its own...",
            "Are you sure you know how to play Marble Blast?",
            "You are contributing to the increasing water levels in the sea below you way too much!",
            "Look at the bright side, it's part of the learning experience, but it doesn't change the fact that you still suck.",
            "If we ever had a 'You suck' achievement, you'd be having the honour to wear it today.",
            "200 more times to go Out of Bounds before you see this message again. For your sake, try and do better.",
            "\"I didn't play on the computer! It...it was.. my auntie!\" Yeah, right. Admit it, you suck.",
            "Are you having fun going Out of Bounds all the time? It seriously looks like it.",
            "Don't you just hate all these messages that make a mockery of your suckiness? It's a joke of course, but it's a nice easter egg.\nIf you don't want to see them anymore, then stop going Out of Bounds so many times!",
            "My grandmother is better than you!",
            "We'll see what happens first: You finishing the level, or the clock hitting the 100 minute mark.",
            "Can we put this on the video show? I mean, that was absolutely stupid of you to go Out of Bounds like that!",
            "While we're on the subject of you going Out of Bounds, you should try and find out all the possible ways to go Out of Bounds, including the stupid ways which you seem to excel in.",
            "This level isn't made out completely out of tiny thin tightropes! You have no excuse whatsoever on failing this badly. If you see this message on Tightropes, Catwalks or Slopwropes, ignore it. Instead, change it to: hahahahahahahahahaha fail!",
            "Excuse of the Day: \"I was pushed Out of Bounds by an invisible Mega Marble!\"",
            "Congratulations, you win--- wait, no, no you don't. You went Out of Bounds. Sorry, you lose. Again.",
            "I found a way for you not to go Out of Bounds. We'll change the shape of the marble to a cube. Wait, never mind, you'll still find a way, because you can.",
            "You sure you played the beginner levels? You did? Doesn't look like it.",
            "You know what would be hilarious? This message popping up on 'Let's Roll'. I hope you aren't playing that level right now... are you?",
            "Mind if we'll change your name to 'Mr. McFail?'",
            "Excuse of the Day: \"But I was distracted by ________ and he/she/it wouldn't stop and forced me to go Out of Bounds.\"",
            "Which one are you: a bad player or a bad player? We willl go with option C: a really bad player.",
            "Excuse of the Day: WHO PUT THAT GRAVITY MODIFIER IN THERE??!?!",
            "Excuse of the Day: That In Bounds Trigger WAS NOT in the level last time I played it! Somebody hacked the level and put one in there!",
            "Excuse of the Day: My awesome marble was abducted by aliens and was replaced by a really crap one!",
            "Excuse of the Day: That Out of Bounds trigger was NOT there before! I swear!",
            "Excuse of the Day: I'm not Pascal :(",
            "Excuse of the Day: I don't suck, I fell off because I wanted to get to the next 200 Out of Bounds multiplier so I can see the awesome messages that are written down.",
            "You know, you won't beat the level if you keep falling off. You will, however, see more of these messages. Try and stay on the level next time. Our guess is that you can't, because you're bad.",
            "Look at the statistics page! I bet you fell more times than the amount of levels you beat!",
            "Excuse of the Day: I'm learning to play... the hard way.",
            "Apparently your marble isn't supermarble. It is suckmarble.",
            "Foo-Foo Marble laughs at how bad you are.",
            "A Rock Can Do Better!",
            "Please, Quit Embarassing Yourself.",
            "Keep this up and you'll win the 'Award of LOL', courtesy of Marble Blast Fubar creators!",
            "Marble Blast Fubar creators would like to give you the title of 'Official NOOB of the Year'. Congratulations!",
            "Did you hear that 'Practice Makes Perfect'? Apparently not.",
            "You should create a new level and title it 'Learn the In Bounds and Out of Bounds Triggers' because you're so experienced with them.",
            "We've seen the ways you fell while playing this game and we gotta admit, some of their are epic fails. We still can't stop laughing!",
            "SING WITH ME:\n\nOne hundred and ninety nine times Out of Bounds, one hundred and ninety nine times Out of Bounds, throw the marble off the level, two hundred times Out of Bounds!",
            "*sigh*, you just can't stop yourself from going Out of Bounds, can you?",
            "Excuse of the Day: I'm playing one of those special levels from Technostick where you must fall off in order to beat them.",
            "Excuse of the Day: I'm having a bad karma today.",
            "Excuse of the Day: So THAT'S what my astronomer referred to when he said I'll keep falling off today.",
            "What do you have against the marble that you keep making it fall off the level?!",
            "I bet you wish you had a Blast or an Ultra Blast powerup to save you. Perhaps even the World's Greatest Blast. Well, reality to player, reality to player: we don't have such a thing existing in this game, so stop playing so badly!",
            "And how is it OUR fault that you're playing so badly?",
            "Do you ever think about the marble's safety when you're playing? Apparently not because you're really careless with it."
        };

        oobSpecial = new string[12]
        {
            "You went Out of Bounds for 1,250 times. This program will now sit in the corner and cry about how bad you are and hope that when you open it again you won't repeat it. False hopes are still hopes.",
            "You went Out of Bounds for 2,500 times. If you aren't tired of going Out of Bounds all the time, we sure did. Stop it already!",
            "Another 1,250 marbles had fallen to the great sea below, and you've reached the 3,750 Out of Bounds mark. You definitely suck. Ah yes, greenpeace would like to see you in court for your \"contribution\" to rising sea levels.",
            "If I had a nickel for every marble that fell Out of Bounds I'd be rich right now and all thanks to you. However, I'm not going to give you any money. Instead, I'll stick my tongue out at you and then laugh at you. Ah yes, congratulations on hitting the 5,000 Out of Bounds mark.",
            "6,750 times Out of Bounds. Let's assume, hypothetically, that you won't go Out of Bounds ever again. Actually, never mind that, you will still suck even if you don't go Out of Bounds again.",
            "I have an awesome gut feeling that you are going 7,500 times Out of Bounds on purpose if only to see these messages and to hear about how bad you are.\nWell then, I won't keep it away from you.\nYou suck!",
            "8,750 times Out of Bounds. For reaching this landmark, I'm giving you a nice Australian Slang sentence to answer the question: Will you ever stop sucking in this game and go Out of Bounds? Answer:\nTill it rains in Marble Bar\n\n\nIn your language it means:\nNever.",
            "Wow, you truly are bad, probably one of the worst Marble Blast players to ever live on this planet. Or you just keep failing to good runs. Are you sure you aren't playing an easy level while this message pops up? Whatever, those messages will now repeat themselves (with a few exceptions), but for now, please remember this:\n\n\nYOU suck!",
            "SING WITH ME:\n\nForty nine thousand nine hundred and ninety nine times Out of Bounds, forty nine thousand nine hundred and ninety nine times Out of Bounds, knock a marble off the level, fifty thousand times Out of Bounds!",
            "What's that in the sky? Is it a plane? Is it a bird? No! It's the marble! And it's way off the level!!! Congratulations on hitting 300,000 Out of Bounds mark. You may now suck more.",
            "1,000,000 times Out of Bounds?!?! You seriously love this game, don't you? Well then, thanks for playing Marble Blast Platinum! Please keep this bad playing up and continue to go Out of Bounds. We'll just laugh at how bad you are. Also, this is the final message as from now on they're all repeats. Thank you for sucking at Marble Blast Platinum!",
            "You have no life. This is official."
        };
    }
}
