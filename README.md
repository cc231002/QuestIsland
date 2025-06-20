# QuestIsland
A simple 3D game made with Unity that includes general education and trivia

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


