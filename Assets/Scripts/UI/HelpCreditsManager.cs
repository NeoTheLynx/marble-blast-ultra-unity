using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpCreditsManager : MonoBehaviour
{
    int page;
    int maxPage = 50;

    public Button next;
    public Button prev;
    public Button home;
    public TextMeshProUGUI pageNumber;
    [Space]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI captionText;
    [SerializeField] float margin;
    [Space]
    public GameObject HC_StartPad;
    public GameObject HC_EndPad;
    public GameObject HC_Gem1;
    public GameObject HC_Gem2;
    public GameObject HC_Gem3;
    public GameObject HC_SuperSpeed;
    public GameObject HC_SuperJump;
    public GameObject HC_ShockAbsorber;
    public GameObject HC_AntiGravity;
    public GameObject HC_Helicopter;
    public GameObject HC_TimeTravel;
    public GameObject HC_DuctFan;
    public GameObject HC_Tornado;
    public GameObject HC_Trapdoor;
    public GameObject HC_Oilslick;
    public GameObject HC_Landmine;
    public GameObject HC_Bumper;
    public GameObject HC_SuperBounce;
    public GameObject HC_Teleport;
    public GameObject HC_RandomPowerup;
    public GameObject HC_EasterEgg;
    public GameObject HC_Checkpoint;
    public GameObject HC_Ball29;
    public GameObject HC_Ball32;
    public GameObject HC_Ball8;
    public GameObject HC_Small2;
    public GameObject HC_Big1;
    public GameObject names_right;
    public GameObject names_center;
    public GameObject marbles_text;

    float initOffsetX, initOffsetY;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //JukeboxManager.instance.PlayMusic("Pianoforte");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void UpdateHUDMaterial()
    {
        int targetLayer = LayerMask.NameToLayer("HUD");
        float smoothness01 = 1f;

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].layer != targetLayer)
                continue;

            Renderer[] renderers = allObjects[i].GetComponentsInChildren<Renderer>(true);

            for (int r = 0; r < renderers.Length; r++)
            {
                // This creates per-renderer material instances
                Material[] mats = renderers[r].materials;

                for (int m = 0; m < mats.Length; m++)
                {
                    Material mat = mats[m];
                    if (mat == null) continue;

                    if (mat.HasProperty("_Smoothness"))
                        mat.SetFloat("_Smoothness", smoothness01);

                    if (mat.HasProperty("_Glossiness"))
                        mat.SetFloat("_Glossiness", smoothness01);
                }
            }
        }
    }

    public void Start()
    {
        //JukeboxManager.instance.PlayMusic("Quiet Lab");

        Time.timeScale = 1f;
        UpdateHUDMaterial();

        next.onClick.AddListener(NextPage);
        prev.onClick.AddListener(PrevPage);
        home.onClick.AddListener(() =>
        {
            //JukeboxManager.instance.PlayMusic("Pianoforte");
            SceneManager.LoadScene("MainMenu");
        });

        initOffsetX = captionText.GetComponent<RectTransform>().offsetMin.x;
        initOffsetY = captionText.GetComponent<RectTransform>().offsetMin.y;

        page = 0;
        SetPage(page);
    }

    void NextPage()
    {
        page++;
        if (page >= maxPage)
            page = 0;
        SetPage(page);
    }

    void PrevPage()
    {
        page--;
        if (page < 0)
            page = maxPage - 1;
        SetPage(page);
    }

    void SetMargin(float marginLeft, float marginTop) => captionText.GetComponent<RectTransform>().offsetMin = new Vector2(initOffsetX + marginLeft, initOffsetY + marginTop);

    public void SetPage(int _page)
    {
        pageNumber.text = "Page " + (_page + 1) + " of " + maxPage;

        names_right.SetActive(false);
        names_center.SetActive(false);
        marbles_text.SetActive(false);

        HC_StartPad.SetActive(false);
        HC_EndPad.SetActive(false);
        HC_Gem1.SetActive(false);
        HC_Gem2.SetActive(false);
        HC_Gem3.SetActive(false);
        HC_SuperSpeed.SetActive(false);
        HC_SuperJump.SetActive(false);
        HC_ShockAbsorber.SetActive(false);
        HC_AntiGravity.SetActive(false);
        HC_Helicopter.SetActive(false);
        HC_TimeTravel.SetActive(false);
        HC_DuctFan.SetActive(false);
        HC_Tornado.SetActive(false);
        HC_Trapdoor.SetActive(false);
        HC_Oilslick.SetActive(false);
        HC_Landmine.SetActive(false);
        HC_Bumper.SetActive(false);
        HC_SuperBounce.SetActive(false);
        HC_Teleport.SetActive(false);
        HC_RandomPowerup.SetActive(false);
        HC_EasterEgg.SetActive(false);
        HC_Checkpoint.SetActive(false);
        HC_Ball29.SetActive(false);
        HC_Ball32.SetActive(false);
        HC_Ball8.SetActive(false);
        HC_Small2.SetActive(false);
        HC_Big1.SetActive(false);

        switch (_page)
        {
            case 0:
                titleText.text = "<size=45>The Background of MBP";
                captionText.text = "<size=30>Marble Blast Platinum was created to be the next Marble Blast game until a new official game is released by GarageGames. Since it requires a full version of Marble Blast Gold installed, it's assumed the player has already played Marble Blast.";
                SetMargin(0, 0);
                break;

            case 1:
                titleText.text = "<size=45>Levels Order in MBP";
                captionText.text = "<size=30>Marble Blast Platinum offers a new, harder challenge. While the beginner levels introduce the player to the basics of Marble Blast, the intermediate, advanced and expert levels require progressively more skills.";
                SetMargin(0, 0);
                break;

            case 2:
                titleText.text = "<size=40>Additional Challenges & Extras (1/3)";
                captionText.text = "<size=24.5>Marble Blast Platinum offers the player several new challenges: To find an Easter Egg (usually an egg-looking shape which is hidden in certain levels) and achieve the Ultimate Time in every level (whereas the Platinum Time seems rather easy, Ultimate Times take your skills to the max! To get Ultimate Times you have to know the right path, do the right tricks, complete the level quickly and on some levels, flawlessly).";
                SetMargin(0, 0);
                break;

            case 3:
                titleText.text = "<size=40>Additional Challenges & Extras (2/3)";
                captionText.text = "<size=30>Another challenge is Achievements. Those achievements are given once a certain goal had been achieved. Press the 'Trophy' button to see which achievements you unlocked and which you still have to work for.";
                SetMargin(0, 0);
                break;

            case 4:
                titleText.text = "<size=40>Additional Challenges & Extras (3/3)";
                captionText.text = "<size=30>The final challenge is the leaderboards, where you can play against anyone in the world in a real-time leaderboards environment. The leaderboards also contain dozens more achievements than in normal gameplay.";
                SetMargin(0, 0);
                break;

            case 5:
                titleText.text = "<size=40>How to Play (1/3)";
                captionText.text = "<size=27>" + Utils.Resolve("If you somehow missed the beginner levels' instructions (which are self-explanatory), you use the <func:bind moveforward>, <func:bind moveleft>, <func:bind moveright> and <func:bind movebackward> keys to roll the marble around and <func:bind jump> to jump. Use <func:bind mouseFire> to use a powerup. As you play through the beginner levels, you'll gain valuable knowledge and skills which will help you to beat harder levels.");
                SetMargin(0, 0);
                break;

            case 6:
                titleText.text = "<size=40>How to Play (2/3)";
                captionText.text = "<size=27>The following pages will detail your goals, bonus items, hazards and other goodies and helpers which you will cross in your path from the first level to the very last. Note that the Random Powerup is not available in the regular mode and can only be found in the Level Editor or in Custom Levels.";
                SetMargin(0, 0);
                break;

            case 7:
                titleText.text = "<size=40>How to Play (3/3)";
                captionText.text = "<size=27>Although Marble Blast Platinum works with joysticks, we believe that there should be more specific support. We found a nice site, <link=\"http://xpadder.com\"><color=#CC3300>Xpadder</color></link>, where you can use anything (such as the xbox360 controller). As such we recommend downloading it as it's free of charge. Marble Blast Platinum comes with joystick input enabled so you can instantly play with your controller!";
                SetMargin(0, 0);
                break;

            case 8:
                HC_StartPad.SetActive(true);
                HC_EndPad.SetActive(true);
                HC_Gem1.SetActive(true);
                HC_Gem2.SetActive(true);
                HC_Gem3.SetActive(true);
                titleText.text = "<size=50>Goals";
                captionText.text = "<size=26>Start Pad - This is where you start the level.\n\nEnd Pad - Get your marble here to end the level.\n\nDiamonds - if a level has diamonds, you must pick them all up before you can finish.";
                SetMargin(margin, 0);
                break;

            case 9:
                HC_SuperSpeed.SetActive(true);
                HC_SuperJump.SetActive(true);
                HC_ShockAbsorber.SetActive(true);
                HC_SuperBounce.SetActive(true);
                titleText.text = "<size=50>Bonus Items (1/3)";
                captionText.text = "<size=26>Speed Booster - gives you a boost of speed.\n\nJump Boost - gives you a huge jump boost.\n\nAnti Recoil - absorbs bounce impacts.\n\nMarble Recoil - makes your marble bounce higher.";
                SetMargin(margin, 0);
                break;

            case 10:
                HC_AntiGravity.SetActive(true);
                HC_Helicopter.SetActive(true);
                HC_TimeTravel.SetActive(true);
                titleText.text = "<size=50>Bonus Items (2/3)";
                captionText.text = "<size=26>Helicopter - Gives your marble wings!\n\nTime Modifier - Deducts some time from the clock!\n\nGravity Defier - Changes the direction of gravity to the direction of the arrow.";
                SetMargin(margin, 0);
                break;

            case 11:
                HC_RandomPowerup.SetActive(true);
                titleText.text = "<size=50>Bonus Items (3/3)";
                captionText.text = "<size=26>Random Powerup - Roll over this and you will receive one of six powerups: Jump Boost, Speed Booster, Helicopter, Anti-Recoil, Marble Recoil, or Time Modifier! Beware - the Random Powerup will NOT respawn if you recieve a Time Modifier.";
                SetMargin(margin, 0);
                break;

            case 12:
                HC_Teleport.SetActive(true);
                titleText.text = "<size=40>Other Goodies and Helpers (1/2)";
                captionText.text = "<size=26>Teleporter - Teleports the marble to a specified destination.";
                SetMargin(12, 130);
                break;

            case 13:
                HC_EasterEgg.SetActive(true);
                HC_Checkpoint.SetActive(true);
                titleText.text = "<size=40>Other Goodies and Helpers (2/2)";
                captionText.text = "<size=26>Easter Egg - A sneaky little egg which is hidden in levels.\n\nCheckpoint - Respawns the marble back on it when it falls Out of Bounds so you don't have to do the whole level again.";
                SetMargin(margin, 0);
                break;

            case 14:
                HC_DuctFan.SetActive(true);
                HC_Tornado.SetActive(true);
                HC_Trapdoor.SetActive(true);
                titleText.text = "<size=50>Hazards (1/2)";
                captionText.text = "<size=27>Fan - Blows the marble away with chilling air!<size=35>\n\n<size=27>Cyclone - A Marble Sucker!<size=35>\n\n<size=27>Trapdoor - Stay on it too long and you will fall.";
                SetMargin(margin, 0);
                break;

            case 15:
                HC_Bumper.SetActive(true);
                HC_Landmine.SetActive(true);
                HC_Oilslick.SetActive(true);
                titleText.text = "<size=50>Hazards (2/2)";
                captionText.text = "<size=27>Bumper - Touch it and be bounced away!<size=35>\n\n<size=27>Nuke - We'll have a big explosion!<size=35>\n\n<size=27>Magnet - Marbles really get attracted by it.";
                SetMargin(margin, 0);
                break;

            case 16:
                titleText.text = "<size=50>Additional Friction Floors";
                captionText.text = "<size=30>Marble Blast Platinum features new floors with distinct properties. For example, while a marble might keep its momentum on ice, it will come to an immediate stop on carpet and rug. Additional surfaces are water, sand, tarmac and others.";
                SetMargin(0, 0);
                break;

            case 17:
                titleText.text = "<size=40>Level Selection Screen (1/8)";
                captionText.text = "<size=27>When going to the level selection screen, you'll find 6 new buttons to the right and one to the bottom left. In order from top to bottom they are: Marble Selection (for custom marbles), Statistics, Achievements, Level Search, Level Editor and Demo Recorder. On the left: Switch Game.";
                SetMargin(0, 0);
                break;

            case 18:
                marbles_text.SetActive(true);
                HC_Ball8.SetActive(true);
                HC_Ball29.SetActive(true);
                HC_Ball32.SetActive(true);
                HC_Big1.SetActive(true);
                HC_Small2.SetActive(true);
                captionText.text = "";
                titleText.text = "<size=40>Level Selection Screen (2/8)";
                break;

            case 19:
                titleText.text = "<size=40>Level Selection Screen (3/8)";
                captionText.text = "<size=30>The statistics page gives you some cool statistics such as how many levels completed, Easter Eggs found, times Out of Bounds, total time of playing MBP and total time for all official levels completed.";
                SetMargin(0, 0);
                break;

            case 20:
                titleText.text = "<size=40>Level Selection Screen (4/8)";
                captionText.text = "<size=30>The achievements page shows which of the 8 original achievements you have unlocked. The first 6 achievements are not found in Leaderboards, whereas the other two, Easter Eggs achievements, are shared with them.";
                SetMargin(0, 0);
                break;

            case 21:
                titleText.text = "<size=40>Level Selection Screen (5/8)";
                captionText.text = "<size=27>The level search button allows you to look for, and immediately load and play, any unlocked MBP official level, MBG level or custom level. So if you have over 500 levels and you're looking for a specific one, the level search button will make short work of that.";
                SetMargin(0, 0);
                break;

            case 22:
                titleText.text = "<size=40>Level Selection Screen (6/8)";
                captionText.text = "<size=30>Pressing the Level Editor button once enables the level editor and thus you no longer need to type in any code for it. It also globalizes which key needs to be pressed, so it's the same for both PC, MAC and Linux; it's the F11 button.";
                SetMargin(0, 0);
                break;

            case 23:
                titleText.text = "<size=40>Level Selection Screen (7/8)";
                captionText.text = "<size=27>Pressing the 'camera' button immediately enables the code that allows you to record levels and note the author and give a description to the level recorded. The Replay Centre can be used to immediately replay demos made by you or other people.";
                SetMargin(0, 0);
                break;

            case 24:
                titleText.text = "<size=40>Level Selection Screen (8/8)";
                captionText.text = "<size=30>Pressing the 'Switch Game' button switches immediately between Marble Blast Gold levels and Marble Blast Platinum levels. In MBG the achievements button does not exist but unlike MBP custom levels are shown.";
                SetMargin(0, 0);
                break;

            case 25:
                titleText.text = "<size=40>Leaderboards (1/5)";
                captionText.text = "<size=27>The latest major addition to MBP are the leaderboards. Players can play on five different leaderboards, although two of them are exclusive to a certain marble: GarageGames. Those two leaderboards feature the same levels as the original two but contain several different levels (MBP only).";
                SetMargin(0, 0);
                break;

            case 26:
                titleText.text = "<size=40>Leaderboards (2/5)";
                captionText.text = "<size=27>Leaderboards now contain as many as 30 achievements for players to unlock, spanning every available leaderboard. Each achievement unlocked awards players 'Award Points' that contribute to their overall rating, thus helping them achieve higher global rank. Award Points are not displayed to the player.";
                SetMargin(0, 0);
                break;

            case 27:
                titleText.text = "<size=40>Leaderboards (3/5)";
                captionText.text = "<size=30>Custom leaderboards only have normal sized marbles and contribute towards the overall rating. Hint: Some custom levels contain Easter Eggs! Each Easter Egg found awards points which go toward the overall rating.";
                SetMargin(0, 0);
                break;

            case 28:
                titleText.text = "<size=40>Leaderboards (4/5)";
                captionText.text = "<size=30>The leaderboards contain new exclusive sounds. We recommended lowering the music volume so that you can hear the achievement sounds and other chat sounds. Some of these sounds can only be heard at game end.";
                SetMargin(0, 0);
                break;

            case 29:
                titleText.text = "<size=40>Leaderboards (5/5)";
                captionText.text = "<size=27>Please read the 'Game Crash/Level Doesn't Load' section for information about the Anti-Hack system implemented for the leaderboards and how to avoid triggering it. Other anti-cheating codes are the disabling of the console and level editor during leaderboards sessions.";
                SetMargin(0, 0);
                break;

            case 30:
                titleText.text = "<size=40>Patch 1.10 (1/4)";
                captionText.text = "<size=27>The eagerly awaited Marble Blast Platinum release Version 1.10 contains over 150 new fixes and additions to Marble Blast Platinum and Gold. Many of these are level-specific, so it's a good idea to check the patch notes for 1.10 for any changes in levels.";
                SetMargin(0, 0);
                break;

            case 31:
                titleText.text = "<size=40>Patch 1.10 (2/4)";
                captionText.text = "<size=30>The remainder of the fixes/additions for Marble Blast Platinum enhance gameplay in many aspects as well as allow the player to customize the game further. For example, players can play in any window size by setting a custom resolution.";
                SetMargin(0, 0);
                break;

            case 32:
                titleText.text = "<size=40>Patch 1.10 (3/4)";
                captionText.text = "<size=30>Thanks to the community's suggestion and responses, we could fix many bugs and add new content such as a cancel button for the demo recorder and the previously mentioned Custom Resolution.";
                SetMargin(0, 0);
                break;

            case 33:
                titleText.text = "<size=40>Patch 1.10 (4/4)";
                captionText.text = "<size=30>Another noteworthy addition is the new soundtracks! As well as the 'Director's Cut' songs, now automatically given in this build, 4 new and 4 old songs are released: the 4 Marble Blast Gold music tracks and 4 new tracks from Phil!";
                SetMargin(0, 0);
                break;

            case 34:
                titleText.text = "<size=40>The Jukebox! (1/2)";
                captionText.text = "<size=27>For Patch 1.10 we've added a Jukebox which can be popped up when pressing F5 on the keyboard. Press F6 to go back a song, F8 to go to the next song and F7 to Play/Stop the song.";
                SetMargin(0, 0);
                break;

            case 35:
                titleText.text = "<size=40>The Jukebox! (2/2)";
                captionText.text = "<size=27>It is possible to add more songs! Simply take your favourite songs (in WAV/MP3 format), convert them into OGG format (we recommend <link=\"http://audacity.sourceforge.net\"><color=#CC3300>Audacity</color></link> for both Windows and Macintosh free of charge), then put them in the folder marble/data/sound and enjoy them in-game!";
                SetMargin(0, 0);
                break;

            case 36:
                titleText.text = "<size=40>Marble/data folder changes (1/2)";
                captionText.text = "<size=30>In order to provide a non-cheating as well as organised environment, we rearranged the data folder.However, to support pre - existing custom levels(as well as future ones), we have included the old interiors folder.";
                SetMargin(0, 0);
                break;

            case 37:
                titleText.text = "<size=40>Marble/data folder changes (2/2)";
                captionText.text = "<size=27>However, players must note that the interiors folder doesn't have anything, so they should download the <link=\"http://www.marbleblast.com/index.cgi?board=levelbuildingboard&action=display&thread=6074\"><color=#CC3300>CLA Interiors</color></link> packages that will enable them to play just about every custom level out there. Note that the link also contains custom levels available to download.";
                SetMargin(0, 0);
                break;

            case 38:
                titleText.text = "<size=40>Hidden Easter Eggs";
                captionText.text = "<size=27>Marble Blast Platinum staff left behind quite a few features and surprises ('Easter Eggs') which can be found during gameplay scattered around, such as the Random Powerup and the Teleporter Delay Timer. Doing things that are usually not meant to be done might yield some other Easter Eggs.";
                SetMargin(0, 0);
                break;

            case 39:
                titleText.text = "<size=40>Game Crash/Level doesn't load (1/4)";
                captionText.text = "<size=27>If the level loading bar does not load fully, check Console.log for any error then fix it in the corresponding mission file. The game could crash due to Mines/Nukes, Moving Platforms not done correctly or if the level exceeds the interior limits of 512 for PC or over 1000 for the Mac.";
                SetMargin(0, 0);
                break;

            case 40:
                titleText.text = "<size=40>Game Crash/Level doesn't load (2/4)";
                captionText.text = "<size=23>If crashing persists:\n- Delete mines/nukes\n- Find the codes for moving platforms and fix them in the corresponding mission file.\n-Upon starting MBP, do not play any other level before this one, and ensure that the level does not contain over 512 interiors. If the game keeps crashing afterwards, please post the level name at the forums (link at the bottom of the main menu) and we'll fix it.";
                SetMargin(0, 0);
                break;

            case 41:
                titleText.text = "<size=40>Game Crash/Level doesn't load (3/4)";
                captionText.text = "<size=30>Another possible cause is the Anti-Hack device, which has been placed over all leaderboards missions. This device prevents you from playing any leaderboards level that you modify. It applies equally to custom levels and all interiors";
                SetMargin(0, 0);
                break;

            case 42:
                titleText.text = "<size=40>Game Crash/Level doesn't load (4/4)";
                captionText.text = "<size=27>There is no way around it; if you were foolish enough to have changed a mission/interior, you have to re-install Marble Blast Platinum or find someone from the forums to send you the level to replace. If it's a custom level, re-install that level from the download section.";
                SetMargin(0, 0);
                break;
            
            case 43:
                titleText.text = "<size=50>About GMD";
                captionText.text = "<size=32>GMD (Game Modifier Developers) was established by a group of teenagers who wanted to change the way Marble Blast looks and feels. Their mission is to create a better gameplay for all players.";
                SetMargin(0, 0);
                break;

            case 44:
                names_right.SetActive(true);
                names_center.SetActive(true);
                titleText.text = "<size=50>The Marble Blast Platinum Team";
                captionText.text =
                    "<size=27>" +
                    "<link=\"http://marbleblast.com/index.cgi\"><color=#CC3300>Matan Weissman</color></link>\n" +
                    "<link=\"http://profile.myspace.com/index.cfm?fuseaction=user.viewprofile&amp;friendID=109026174\"><color=#CC3300>Spy47</color></link>\n" +
                    "<link=\"http://marbleblast.com/index.cgi?action=viewprofile&amp;user=pablovasquez\"><color=#CC3300>Pablo Vasquez</color></link>\n" +
                    "<link=\"http://www.perishingflames.com\"><color=#CC3300>PerishingFlames</color></link>\n" +
                    "<link=\"http://marbleblast.com/index.cgi?action=viewprofile&amp;user=oakster\"><color=#CC3300>Oakster</color></link>";
                SetMargin(30, 0);
                break;

            case 45:
                titleText.text = "<size=50>Special Thanks (1/4)";
                captionText.text = "<size=28>We would like to thank the community for supporting us in the creation of Marble Blast Platinum, <link=\"http://www.garagegames.com/my/home/view.profile.php?qid=5263\"><color=#CC3300>Alex Swanson</color></link> for his great help with several codes that are now operating in the game, to our tester <link=\"http://marbleblast.com/index.cgi?action=viewprofile&amp;user=moshe\"><color=#CC3300>Moshe</color></link> and to our parents for letting us play and build Marble Blast Platinum!";
                SetMargin(0, 0);
                break;

            case 46:
                titleText.text = "<size=50>Special Thanks (2/4)";
                captionText.text = "<size=30>We'd also like to thank <link=\"http://www.myspace.com/novamusick\"><color=#CC3300>Beau</color></link> for some of the sounds and songs used and <link=\"http://Cyberfox.sytes.net\"><color=#CC3300>CyberFox</color></link> for the sweet new DTS models! As well as <link=\"http://www.freewebs.com/shadowmarble/\"><color=#CC3300>ShadowMarble</color></link> for several portions of the 'Level Completion' code.";
                SetMargin(0, 0);
                break;

            case 47:
                titleText.text = "<size=50>Special Thanks (3/4)";
                captionText.text = "<size=27>And to our staff Matan, Phil and Spy47 for contributing the most to this project, Jase for texturing and great levels, Andrew Sears and Ian for tons of outstanding levels and Spy47 (again, because he's awesome and you know it) for amazing programming work that can be seen throughout the game.";
                SetMargin(0, 0);
                break;

            case 48:
                titleText.text = "<size=50>Special Thanks (4/4)";
                captionText.text = "<size=27>Also to Perishingflames, Lonestar, Technostick & Oakster for testing and pointing out many bugs & mistakes throughout the development of MBP and finally to Pablo for the best, fastest and most reliable QuArK work to ever be seen.\nAnd lastly to <link=\"http://www.GarageGames.com\"><color=#CC3300>GarageGames</color></link> for this amazing game and for letting us modify it! You guys rock!";
                SetMargin(0, 0);
                break;

            case 49:
                titleText.text = "<size=50>Press The Red!";
                captionText.text = "<size=27>When you read through the manual you probably noticed names in <link=\"http://www.youtube.com/IsraeliRedDragon\"><color=#CC3300>red</color></link>. Press them to be linked straight to the webpage attached to them. For example, pressing Phil's red name will link you to his chosen webpage, <link=\"http://philsempire.com\"><color=#CC3300>philsempire.com</color></link>.";
                SetMargin(0, 0);
                break;
        }
    }
}
