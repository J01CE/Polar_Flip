# PolarFlipV2

> A 2D Unity puzzle-platformer built around magnetic polarity mechanics — the player flips their charge to attract or repel from magnetic objects in the environment.

---

## 🎮 Game Overview

**PolarFlipV2** is a 2D side-scrolling game where the player controls a magnetic ball. By flipping the ball's polarity (positive ↔ negative), the player interacts with fixed magnetic objects placed throughout each level. Same polarities repel; opposite polarities attract — mastering this mechanic is the key to progressing through the levels.

---

## 🛠️ Tech Stack

| Property | Value |
|---|---|
| Engine | Unity **6000.3.10f1** (Unity 6) |
| Render Pipeline | Universal Render Pipeline (URP) 17.3.0 |
| Scripting | C# |
| Input System | Unity New Input System 1.18.0 |
| UI | Unity UGUI 2.0.0 + TextMesh Pro |

---

## 📁 Directory Structure

```
PolarFlipV2/
├── Assets/
│   ├── Scrpits/                  # All game C# scripts
│   │   ├── MagneticBall.cs       # Player ball — polarity, physics, visuals
│   │   ├── MagneticObject.cs     # Static magnetic platform/object
│   │   ├── MovementController.cs # Horizontal movement + sprite flip
│   │   └── MainMenu.cs           # Play / Quit scene management
│   ├── Scenes/
│   │   ├── MainMenu.unity        # Title / main menu scene
│   │   ├── l1.unity              # Level 1
│   │   ├── l2.unity              # Level 2
│   │   ├── l3.unity              # Level 3
│   │   └── l4.unity              # Level 4
│   ├── Settings/
│   │   ├── UniversalRP.asset     # URP pipeline asset
│   │   └── Renderer2D.asset      # 2D renderer settings
│   ├── game asset in unity/      # All art & sprite assets
│   └── TextMesh Pro/             # TMP font & shader assets
├── Packages/
│   └── manifest.json             # Unity package dependencies
├── ProjectSettings/              # Unity editor/project configuration
├── PolarFlipV2.sln               # Visual Studio solution
└── README.md
```

---

## 📜 Scripts Reference

### `MagneticBall.cs`
**Attached to:** the player ball GameObject (tagged `"ball"`)

The core player controller. Manages polarity state, magnetic physics, and visual color feedback.

| Field | Type | Default | Description |
|---|---|---|---|
| `isPositive` | `bool` | `true` | Current polarity of the ball |
| `triggerHeight` | `float` | `4f` | Radius within which magnet interaction occurs |
| `hoverHeight` | `float` | `1.2f` | Reserved for future use |
| `attractSpeed` | `float` | `10f` | Attraction force multiplier |
| `repelForce` | `float` | `12f` | Repulsion force multiplier |
| `positiveColor` | `Color` | Blue | Sprite color when polarity is positive |
| `negativeColor` | `Color` | Red | Sprite color when polarity is negative |

**Polarity Physics (direction-aware):**

| Ball | Magnet | Relative Position | Effect |
|---|---|---|---|
| Same | Same | Magnet below | Gravity reduced to `0.4×` + repel force → ball **levitates** above magnet |
| Same | Same | Magnet above | Repel force pushes ball **downward** |
| Different | Opposite | Magnet below | Normal gravity + attract force → ball **snaps to surface** |
| Different | Opposite | Magnet above | Gravity reduced to `0.4×` + attract force → ball **pulled upward** |

**Controls:**
- **F key** — Toggles polarity, applies a small upward velocity kick, updates sprite color

---

### `MagneticObject.cs`
**Attached to:** static magnetic platforms/objects in the level

Represents a stationary magnetic object with a fixed, never-changing polarity. Caches the ball reference on `Start` for performance.

| Field | Type | Description |
|---|---|---|
| `isPositive` | `bool` | Fixed polarity — set per object in the Inspector |
| `ballIsAbove` | `bool` | *(debug)* True when ball's Y > object Y + 0.5 |
| `interacting` | `bool` | *(debug)* True when ball is in the 0.5–4.0 unit zone above |

---

### `MovementController.cs`
**Attached to:** player character

Physics-based horizontal movement using `Rigidbody2D`. Only controls the X axis — vertical movement is handled entirely by gravity and magnetic forces.

| Field | Type | Description |
|---|---|---|
| `speed` | `float` | Movement speed, default `5f` |

- Movement applied in `FixedUpdate` via `rb.linearVelocity` to sync with the physics engine
- `Flip()` mirrors the sprite by toggling `localScale.x` sign while preserving scale magnitude

---

### `MainMenu.cs`
**Attached to:** a GameObject in the `MainMenu` scene

| Method | Description |
|---|---|
| `PlayGame()` | Loads scene at build index `1` (Level 1) asynchronously |
| `QuitGame()` | Calls `Application.Quit()` |

---

## 🗺️ Scenes

| Scene | Purpose |
|---|---|
| `MainMenu` | Title screen with Play and Quit buttons |
| `l1` | Level 1 — primary fully built level |
| `l2` | Level 2 |
| `l3` | Level 3 — fully built level |
| `l4` | Level 4 |

---

## 📦 Key Packages

| Package | Version | Purpose |
|---|---|---|
| `com.unity.render-pipelines.universal` | 17.3.0 | URP rendering |
| `com.unity.inputsystem` | 1.18.0 | New Input System |
| `com.unity.2d.animation` | 13.0.4 | 2D skeletal animation |
| `com.unity.2d.sprite` | 1.0.0 | Sprite tools |
| `com.unity.2d.tilemap` | 1.0.0 | Tilemap support |
| `com.unity.2d.spriteshape` | 13.0.0 | Spline-based terrain |
| `com.unity.ugui` | 2.0.0 | Unity UI (Canvas system) |
| `com.unity.timeline` | 1.8.10 | Timeline animations |

---

## 🚀 Getting Started

1. Open the project in **Unity 6000.3.10f1** (or later Unity 6 patch)
2. Open `Assets/Scenes/MainMenu.unity`
3. Press **Play** in the Editor, or build via **File → Build Settings**

**In-game controls:**

| Key | Action |
|---|---|
| `A` / `←` | Move left |
| `D` / `→` | Move right |
| `F` | Flip magnetic polarity (blue ↔ red) |

---

## 🎨 Polarity Quick Reference

- 🔵 **Blue (Positive)** — same-polarity magnets **repel** the ball (levitation)
- 🔴 **Red (Negative)** — opposite-polarity magnets **attract** the ball (snaps to surface)
- Press **F** to flip between polarities mid-level to navigate obstacles
