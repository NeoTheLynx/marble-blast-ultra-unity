# Marble Blast Platinum Unity Port

The Unity Port of Marble Blast Platinum, based on [Marble Blast Gold Unity remake](https://github.com/NaCl586/marble-blast-gold-unity/) that I did. This remake is based on Marble Blast Platinum 1.14. 
These features are not yet implemented (and might possibly not implemented later): Replay Center, Leaderboards, and Level Editor (this is not planned, so don't ask for level editor).

<img src="https://i.imgur.com/j1YrNlX.png" width="640">
<img src="https://i.imgur.com/yyTlJ6q.png" width="640">
<img src="https://i.imgur.com/WEB3hS8.png" width="640">
<img src="https://i.imgur.com/3HdzfdO.png" width="640">

As the time of me writing this, the game has not been fully tested as I don't have the skill and capability to complete all levels.
Known issues:
- Checkpoint respawn orientation when being upside down is wrong (for this reason, Space Station mis file is changed)
- Some interiors are not parsed correctly into the game (for this reason, Space Station interior is split into two)
- You can't scroll scrollbars using mouse wheel 
- Some Director's Cut levels can cause crash when loading (e.g. Cubed Maze), and some are impossible (e.g. Getting Squeezed, Bent Reality)

Please report other bugs that you find! Thank you

Also lightning is still not optimized enough, feel free to give feedback too

## Download the Windows build [here](https://github.com/NaCl586/marble-blast-platinum-unity/releases/)

If you find bugs or things that are not faithful with the original Marble Blast Platinum, feel free to message me on discord NaCl586#8479.

Special thanks to Vani and RandomityGuy for helping me whenever I have problems when making this project.

## Additional Controls

Press R for quick respawn, works when the game is paused. This button currently is not remappable because I wanted to create the same UI remake without additional things. Also, setting video driver and color mode is just pure cosmetic and does not work.

## Save Data

<img src="https://i.imgur.com/u2wAziG.png" width="640">

Save data uses [PlayerPrefs](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PlayerPrefs.html), which can be accessed via Registry Editor (see picture). If you wanna unlock the levels, you can create a key or edit existing key called "QualifiedLevel[Difficulty][Game]" to a large integer like 9999. The PlayerPrefs essentially is equivalent to prefs.cs in vanilla Marble Blast.

## Custom Level Support

You can add custom levels that are specifically made for Marble Blast Gold and Platinum by placing the mission file in Marble Blast Platinum 1.14_Data\StreamingAssets\marble\data\missions\custom and the interior file in Marble Blast_Data\StreamingAssets\marble\data\interiors (or wherever you put your .dif files when making the level). Adding new folders or custom levels that are not made for Marble Blast Gold do not work. You can technically add more levels to the main game with the same fashion. This feature is theoretically working but still untested.

## Custom Marble Support

Identical with the original MBP 1.14, you can modify a marble texture in the Marble Blast Platinum 1.14_Data\StreamingAssets\marble\custom_marbles folder. Make sure to have the exact same name as the original file when you are changing skins.
