# ReadMe
There are two branches:
- Master
- Photon

## Master
This branch contains the base game without multiplayer. The player spawns in as a knight on the lower floor by default. This can be changed by switching the `Player Type` variable on the `GameManager` script component on the `Game Manager` object in the scene hierarchy. Enemies that are spawned in correspond to the selected `Player Type`.

By default, enemies are not hidden from their target. To toggle this, open the enemy's prefab in the `Resources` folder and select the `Controller` child object, then tick the `Is Hidden From Target` checkbox on the `EnemyController` script component in the inspector. Note that if the game is already running, existing enemies will not be affected.

## Photon
This branch contains the base game with multiplayer. To test the multiplayer on a single computer:
1. Build and run the game (File > Build And Run)
2. In the editor, open the `Loading` scene by double-clicking it in the `Scenes` folder
3. Start the game in the editor
4. Create a room in either the editor or the built game
5. Join a room with the same name in the other instance of the game

Enemies will only start spawning once two players are in the room.