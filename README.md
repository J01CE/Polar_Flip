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
│   │   ├── MovementController.cs # Horizontal/vertical movement + sprite flip
│   │   └── MainMenu.cs           # Play / Quit scene management
│   ├── Scenes/
│   │   ├── MainMenu.unity        # Title / main menu scene
│   │   ├── l1.unity              # Level 1 (fully built, ~77 KB)
│   │   ├── l2.unity              # Level 2
│   │   ├── l3.unity              # Level 3 (fully built, ~49 KB)
│   │   └── l4.unity              # Level 4
│   ├── Settings/
│   │   ├── UniversalRP.asset     # URP pipeline asset
│   │   ├── Renderer2D.asset      # 2D renderer settings
│   │   └── Lit2DSceneTemplate    # Default scene template
│   ├── game asset in unity/      # All art & sprite assets
│   │   ├── backgroung.jpg        # Level background
│   │   ├── charater.png          # Player sprite
│   │   ├── char gemini.png       # Alternate character sprite (Gemini-generated)
│   │   ├── magnets.png           # Magnet object sprite sheet
│   │   ├── flags.png             # Level flag/goal sprite
│   │   ├── menu.png              # Main menu background
│   │   ├── play.png / quit.png   # UI button sprites
│   │   ├── title card.png        # Title screen card
│   │   ├── titlesubwobg.png      # Title subtitle (no background)
│   │   ├── nums-removebg-preview.png  # Number sprites (score / level)
│   │   ├── slab_crop-removebg-preview.png # Platform/slab sprite
│   │   ├── lvl_with_bg-removebg-preview.png # Level selection graphic
│   │   ├── loadgame.png / new game.png / settings.png # Menu UI buttons
│   │   └── bounce.physicsMaterial2D   # Bouncy physics material
│   ├── DefaultVolumeProfile.asset
│   ├── InputSystem_Actions.inputactions  # Input bindings
│   └── UniversalRenderPipelineGlobalSettings.asset
├── Packages/
│   └── manifest.json             # Unity package dependencies
├── ProjectSettings/              # Unity editor/project configuration
│   ├── ProjectVersion.txt        # Unity version: 6000.3.10f1
│   └── ... (26 setting assets)
├── PolarFlipV2.sln               # Visual Studio solution
└── Assembly-CSharp.csproj        # C# project file
```

---

## 📜 Scripts Reference

### `MagneticBall.cs`
**Attached to:** the player ball GameObject (tagged `"ball"`)

The core player controller. Manages polarity state, magnetic physics responses, and visual color feedback.

| Field | Type | Default | Description |
|---|---|---|---|
| `isPositive` | `bool` | `true` | Current polarity of the ball |
| `triggerHeight` | `float` | `4f` | Radius within which magnet interaction occurs |
| `hoverHeight` | `float` | `1.2f` | Target hover distance above a magnet |
| `attractSpeed` | `float` | `5f` | Speed of attraction movement |
| `repelForce` | `float` | `15f` | Force applied when repelled |
| `spriteRenderer` | `SpriteRenderer` | — | Visual renderer for color changes |
| `positiveColor` | `Color` | Blue-ish | Color shown when polarity is positive |
| `negativeColor` | `Color` | Red-ish | Color shown when polarity is negative |

**Key Behaviours:**
- **`F` key** → toggles polarity, applies a small upward velocity kick, updates sprite color
- **`G` key** → debug toggle for gravity scale (dev/test only)
- `OnDrawGizmosSelected` → draws a wire sphere in the Scene view showing the trigger radius (blue = positive, red = negative)

> **Note:** `FixedUpdate` currently logs gravity scale every frame — this is a temporary debug stub and should be replaced with the magnetic force logic.

---

### `MagneticObject.cs`
**Attached to:** static magnetic platforms/objects in the level

Represents a stationary magnetic object with a fixed, never-changing polarity. Visual color mirrors polarity. Tracks whether the player ball is nearby (read-only debug info in Inspector).

| Field | Type | Description |
|---|---|---|
| `isPositive` | `bool` | Fixed polarity — set per object in the Inspector |
| `spriteRenderer` | `SpriteRenderer` | Renderer for color display |
| `ballIsAbove` | `bool` | *(debug)* True when ball's Y > object Y + 0.5 |
| `interacting` | `bool` | *(debug)* True when ball is in the 0.5–4.0 unit zone above |

**Key Behaviours:**
- Finds the ball each frame via `GameObject.FindWithTag("ball")`
- `OnDrawGizmosSelected` → draws a semi-transparent box (blue/red) above the object in the scene view, showing the interaction zone

---

### `MovementController.cs`
**Attached to:** player character (horizontal/vertical movement)

Handles basic 2D movement input and sprite horizontal flipping.

| Field | Type | Description |
|---|---|---|
| `speed` | `int` | Movement speed, set in Inspector |

**Key Behaviours:**
- Reads `Horizontal` and `Vertical` axes each frame and translates the transform
- `flip()` → mirrors the sprite horizontally by setting `localScale.x` to ±7 based on movement direction

---

### `MainMenu.cs`
**Attached to:** a GameObject in the `MainMenu` scene

Handles main menu button callbacks.

| Method | Description |
|---|---|
| `PlayGame()` | Loads scene at build index `1` (Level 1) asynchronously |
| `QuitGame()` | Calls `Application.Quit()` |

---

## 🗺️ Scenes

| Scene | File Size | Purpose |
|---|---|---|
| `MainMenu` | ~5.8 KB | Title screen with Play and Quit buttons |
| `l1` | ~77 KB | Level 1 — primary, fully populated level |
| `l2` | ~5.8 KB | Level 2 |
| `l3` | ~49 KB | Level 3 — fully populated level |
| `l4` | ~5.8 KB | Level 4 |

Scenes `l2` and `l4` are similar in size to the empty MainMenu scene, suggesting they may be placeholders or minimally built.

---

## 📦 Key Packages

| Package | Version | Purpose |
|---|---|---|
| `com.unity.render-pipelines.universal` | 17.3.0 | URP rendering |
| `com.unity.inputsystem` | 1.18.0 | New Input System |
| `com.unity.2d.animation` | 13.0.4 | 2D skeletal animation |
| `com.unity.2d.sprite` | 1.0.0 | Sprite tools |
| `com.unity.2d.tilemap` | 1.0.0 | Tilemap support |
| `com.unity.2d.tilemap.extras` | 6.0.1 | Extra tilemap brushes |
| `com.unity.2d.spriteshape` | 13.0.0 | Spline-based terrain |
| `com.unity.2d.aseprite` | 3.0.1 | Aseprite import support |
| `com.unity.ugui` | 2.0.0 | Unity UI (Canvas system) |
| `com.unity.visualscripting` | 1.9.9 | Visual scripting (Bolt) |
| `com.unity.timeline` | 1.8.10 | Timeline animations |
| `com.unity.test-framework` | 1.6.0 | Unit testing |
| `com.unity.collab-proxy` | 2.11.3 | Unity Version Control |

---

## 🎨 Art Assets

All sprites and textures are stored in `Assets/game asset in unity/`:

| Asset | Description |
|---|---|
| `backgroung.jpg` | Level background image |
| `charater.png` | Main player character sprite |
| `char gemini.png` | AI-generated alternate character (Gemini) |
| `chat gpt.png` / `gpt carete-...png` | AI-generated character variants (ChatGPT) |
| `magnets.png` | Magnet object sprite |
| `flags.png` | Goal/flag sprite |
| `slab_crop-removebg-preview.png` | Platform slab sprite |
| `lvl_with_bg-removebg-preview.png` | Level select graphic |
| `title card.png` / `titlesubwobg.png` | Title screen assets |
| `menu.png` | Main menu background |
| `play.png` / `quit.png` | Menu button sprites |
| `new game.png` / `loadgame.png` / `settings.png` | Additional menu UI buttons |
| `nums-removebg-preview.png` | Number sprites for scoring/levels |
| `bounce.physicsMaterial2D` | Physics material with bounce properties |

---

## ⚠️ Known Issues / TODO

- [ ] `FixedUpdate` in `MagneticBall.cs` only logs gravity — magnetic force/hover logic is not yet implemented
- [ ] `MagneticObject.cs` uses `GameObject.FindWithTag("ball")` every frame, which is expensive — should cache the reference
- [ ] Levels `l2` and `l4` appear to be empty/placeholder scenes
- [ ] Script folder is misspelled as `Scrpits` instead of `Scripts`
- [ ] The `G` key gravity debug toggle and all `Debug.Log` calls should be removed before release

---

## 🚀 Getting Started

1. Open the project in **Unity 6000.3.10f1** (or later Unity 6 patch)
2. Open `Assets/Scenes/MainMenu.unity`
3. Press **Play** in the Editor, or build via **File → Build Settings**
4. In-game controls:
   - **Arrow Keys / WASD** — Move
   - **F** — Flip magnetic polarity
   - **G** — *(Debug)* Toggle gravity scale
