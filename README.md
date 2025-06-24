# QuestIsland
Quest Island is a cozy 3D educational trivia game made with Unity, where players explore a tropical island, collect hearts, enter cloud-shaped booths to answer themed questions across six topics. Each booth includes three questions with increasing difficulty levels, while players must also avoid bees that can sting and reduce health.

## 🎮 How to Run Quest Island - User Guideline

### Option 1: Run the Executable

If you only want to play the game (no Unity or setup needed):

1. Download the build from our OneDrive: [Quest Island Executable](https://fhstp-my.sharepoint.com/personal/lbwu_fhstp_ac_at/_layouts/15/onedrive.aspx?id=%2Fpersonal%2Flbwu%5Ffhstp%5Fac%5Fat%2FDocuments%2FFH%2FBCC%2Flecture%2FCCL4%2FSS2025%2FUpload&ga=1) 
2. Extract the `.zip` file
3. Open the extracted folder
4. Double-click on `QuestIsland.exe` to launch the game

---

### Option 2: Run from Source (Unity Project)

If you want to open, test, or edit the game inside Unity:

#### Requirements
- Unity Editor (6000.0.47f or higher)
- Wwise installed and integrated
- Git (to clone the repository)

#### Steps

1. Clone this repository
2. Open **Unity Hub**
3. Click **Open Project** and select the cloned folder
4. Wait for the project to compile
5. Verify Wwise is working:
   - Open the **Wwise Picker** in Unity
   - Re-link the Wwise project if needed
   - Re-generate SoundBanks if missing
6. In the Project tab, open `Assets/Scenes/StartScene.unity`
7. Press **Play** in the Unity Editor to test the game

---

### 🎮 Controls

- `W A S D`: Move
- `Mouse`: Look around
- Avoid bees, collect hearts, and answer trivia questions to win!


## First Mockups

![mockup1](https://github.com/user-attachments/assets/c7a99a3e-28b5-4922-9055-e705bcc9e591)

![mockup2](https://github.com/user-attachments/assets/3bc738ed-6e4d-4b3f-bd9c-362419b4d668)

## System Design

QuestIsland is structured into 4 key scenes, each with a distinct function. Gameplay progression and user interaction are handled through clearly defined systems.

### Scene Structure

1. Main Menu Scene
   - Options: Start Game / Exit
   - Loads the Game Scene when gameplay begins

2. Game Scene
   - Player explores the island and visits trivia booths
   - Randomly roaming enemies cause health loss on contact
   - Entering a trivia booth triggers a transition to the Trivia Scene

3. Trivia Scene
   - Displays 3 multiple-choice questions for the active quest
   - Player selects answers using the keypad
   - Correct completion unlocks the next booth in the Game Scene
   - On failure (wrong answers), health is reduced and the player is sent back

4. End Scene
   - Shows either the Win or Game Over screen
   - If player finishes all 6 trivia quests: shows Win screen
   - If health reaches 0: shows Game Over screen, resets game

### Core Gameplay Flow

- Player moves freely around the island (WASD + Mouse to guide camera)
- Booths unlock one by one based on progression
- Enemies reduce hearts on collision
- Player health is tracked across scenes
- If hearts reach zero → full game restart

### Input Methods

- Keyboard (WASD): Player movement
- Mouse: Camera rotation
- Keypad (1–4): Answer selection during trivia questions

### Game features

1. 3D Modelling & Animation
   - Main character (humanoid): idle, walk, cheer, point somewhere
   - Enemy character: Bee patroling around
   - Friendly NPC: Jeff, friendly guy guides you through the quests
   - Scene props: trivia booths, treasure chest, island with foliage, sand paths and surrounded by a cozy beach

2. Game Audio
   - Background music: chill, island-style loop
   - Sound FX: walking, answering questions, booth interactions, heart loss, correct/wrong answer, enemy ambient sound
   - WWise Integration: All sounds spatialized + Audio feedback based on player actions
   - Finale: Endgame jingle on winning

3. Unity Features
   - Scene Structure: Main Menu(start game/exit), Game Scene, Trivia Scene, End Scene (win/lose screen)
   - State Manager: tracks health and current quest, Object persists between scenes
   - Input Devices: Keyboard for movement, mouse for guiding the camera, keypad for selecting the answers
   - Game Over Logic: lose all heart → restart full game (Keeps implementation simple and avoids data saving complexity)

## System Infrastructure

![SystemDiagram](https://github.com/user-attachments/assets/561c4f34-2f48-4a25-9a07-345c43ac59d1)


