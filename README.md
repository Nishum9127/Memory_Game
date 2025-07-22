# Memory Match Game

A classic card-matching puzzle game developed in Unity.

## Repository
[Click here to view the repo](https://github.com/Nishum9127/Memory_Game)

---

## Game Overview

Memory Match is a Unity-powered memory game where players flip over pairs of cards to find matches. The game supports multiple grid sizes, tracks scores, limits moves, and includes sound effects and game-over logic.

---

## Features

- **Dynamic Grid Support**  
  Choose from 2x2, 2x3, 4x4, or 5x6 layouts, and cards resize automatically.

- **Smooth Gameplay Logic**  
  Cards flip and compare with built-in matching logic and mismatch handling.

- **Move Limitation & Scoring**  
  Each grid size has its own allowed move count. Score increases on correct matches.

- **Game Over & Completion Panels**  
  UI panels appear when player wins or exhausts all moves.

- **Persistent Score Saving**  
  Uses `PlayerPrefs` to track and store best performance per grid type.

- **Sound Integration**  
  Includes card flip, match, mismatch, background music, and button click sounds.

---

## Code Structure

| File                  | Description                                       |
|-----------------------|---------------------------------------------------|
| `GameManager.cs`      | Main logic for grid creation, scoring, moves, etc.|
| `Card.cs`             | Handles individual card behavior and state.       |
| `SaveManager.cs`      | Wrapper for saving/loading data with PlayerPrefs. |
| `SoundManager.cs`     | Controls background music and SFX.                |
| `GridSizeSelector.cs` | UI handler for selecting different grid sizes.    |

---

## 🎛 Grid Settings

| Grid Type | Allowed Moves |
|-----------|----------------|
| 2x2       | 3              |
| 2x3       | 6              |
| 4x4       | 20             |
| 5x6       | 30             |

---

## How to Run

1. Clone this repo:
   ```bash
   git clone https://github.com/Nishum9127/Memory_Game.git
