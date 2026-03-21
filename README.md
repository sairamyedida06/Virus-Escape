# Virus Escape 🧬

A **3D top-down action survival game** built in Unity for mobile (Android), where players must collect all cores scattered across the level to unlock the exit portal — all while being hunted by intelligent enemies and navigating deadly spike traps.

**Available on:**
- 

---

## 🎮 Gameplay

- Navigate a 3D top-down environment using mobile joystick controls
- Collect all **Cores** scattered across the level to unlock the Level Exit portal
- Avoid or survive **NavMesh-driven enemies** that patrol, chase, and attack
- Dodge **Spike Traps** — both pressure-triggered and looping variants
- Manage your **Health** and **Stamina** to survive
- Reach the portal once all cores are collected to progress to the next level

---

## 🏗️ Architecture Overview

Built with a strong focus on **decoupled, modular architecture**. Systems communicate through interfaces and singleton managers rather than direct dependencies. Every system has a single, clear responsibility.

```
VirusEscape/
├── Core/
│   ├── SceneInitializer.cs         # Auto-instantiates managers at runtime
│   ├── Game_Manager.cs             # Scene flow, pause, restart, quit
│   ├── UI_Manager.cs               # All UI state management
│   ├── Audio_Manager.cs            # Music + SFX, scene-aware, persistent
│   └── VFX_Manager.cs              # Centralised VFX spawning, persistent
├── Player/
│   ├── Player.cs                   # Movement, jump, sprint, gravity, camera-relative input
│   ├── Player_Health.cs            # IDamageable, death/damage UnityEvents, fall detection
│   ├── PlayerStamina.cs            # Stamina pool, regen delay, fill amount for UI
│   ├── Player_Animation.cs         # Animator driver — speed, jump, fall, death states
│   ├── Player_Sfx.cs               # SFX bridge — routes jump sound through AudioManager
│   ├── Blink.cs                    # Invincibility blink — timed on/off cycle on mesh targets
│   ├── IDamagables.cs              # IDamageable interface
│   ├── Health_Display.cs           # Dynamic heart icon UI
│   └── Stamina_bar.cs              # Stamina slider with fade logic
├── Enemies/
│   ├── Enemy_Ai.cs                 # NavMesh FSM — Patrol / Chase / Attack
│   └── Crawler_Zombie.cs           # Physics-based Rigidbody enemy
├── Hazards/
│   └── Spike_Trap.cs               # Dual-mode FSM trap (pressure + looping)
├── Collectibles/
│   ├── Collectable.cs              # Trigger, VFX, UnityEvent
│   └── Collectable_animation.cs    # Sine-wave float + rotation animation
├── Level/
│   ├── Level_Exit.cs               # Gate unlock + scene transition
│   ├── Progress_UI.cs              # Coin tracking, gate trigger
│   └── Progress_Display.cs         # Remaining coins UI + success panel
```

---

## ⚙️ Key Systems

### 🚀 Scene Initializer — `SceneInitializer.cs`
The most important architectural decision in the project. Instead of manually placing managers in every scene, a single `SceneInitializer` prefab checks whether each manager already exists and instantiates it if not — then calls the correct UI state for that scene type.

```csharp
// Auto-instantiates only if not already present — no duplicate managers
if (UI_Manager.Instance == null)
{
    Instantiate(uiManager);
    DontDestroyOnLoad(UI_Manager.Instance);
}

// Then sets UI state based on scene type
if (scenetype == Scenetype.Gameplay)
    UI_Manager.Instance.ShowHud();
else
    UI_Manager.Instance.ShowMainMenu();
```

**Why this matters:** Eliminates missing-reference bugs, removes per-scene setup overhead, and makes adding new scenes trivial — just drop in the initializer prefab and set the scene type.

---

### 🧠 Enemy AI — `Enemy_Ai.cs`
Full NavMesh-driven FSM with three states: **Patrol**, **Chase**, and **Attack**. Detection and patrol are both **anchored to the spawn position** — not the current position — preventing enemies from drifting infinitely or detecting the player from across the map.

```
State Machine:
┌─────────┐    player in range     ┌───────┐    in attack range    ┌────────┐
│  Patrol │ ─────────────────────► │ Chase │ ───────────────────► │ Attack │
└─────────┘                        └───────┘                       └────────┘
     ▲                                  │                               │
     │◄─────── out of range ────────────┘◄─────── attack ends ──────────┘
```

Key design details:
- **Spawn-anchored detection** — `playerDistanceFromSpawn <= detectionRadius` prevents detection while roaming
- **Max chase distance** — enemy gives up and returns to patrol if player leads it too far
- **Attack validation** — double-checks distance before committing to an attack swing
- **Animation-driven damage** — `DealDamage()` is called from animation events, not from code timers
- **Gizmos** — patrol radius (green), detection radius (yellow), and max chase distance (blue) all visible in Scene view

---

### 🪤 Spike Trap — `Spike_Trap.cs`
Dual-mode FSM trap decoupled entirely from level geometry. Two trap types configurable in the Inspector:

| Mode | Behaviour |
|---|---|
| `Looping` | Continuously cycles active → idle → active on a timer |
| `Pressure` | Activates only when player enters the trigger zone |

```
FSM States:
Idle → Wait → TransitionToActive → Active → TransitionToIdle → (loop or Idle)
```

- Spike mesh position is **Lerped** between idle and active positions for smooth animation
- Damage is applied via the `IDamageable` interface — trap has zero knowledge of what it's damaging
- A tracked `List<IDamageable>` handles all objects currently inside the trigger, so damage applies correctly on state entry

---

### 🎯 IDamageable Interface — `IDamagables.cs`
The core contract that decouples all damage sources from damage receivers.

```csharp
public interface IDamageable
{
    void Damage(int damage);
}
```

Both `EnemyAI`, `CrawlerZombie`, and `Spike_Trap` deal damage through this interface — none of them know whether they're hitting a player, an NPC, or anything else. Adding a new damageable object requires only implementing this interface.

---

### 🎬 VFX Manager — `VFX_Manager.cs`
Persistent singleton that centralises all VFX spawning. Individual systems (Collectable, Spike_Trap) call into it rather than holding their own prefab references — keeping prefab management in one place.

```csharp
// Collectable just calls this — no prefab reference needed locally
VFX_Manager.Instance.PlayCoinVFX(transform.position);
```

Supports: Land VFX, Coin collection VFX, Spike trap hit VFX.

---

### 🔊 Audio Manager — `Audio_Manager.cs`
Persistent singleton with **scene-aware music logic**. Subscribes to `SceneManager.sceneLoaded` to automatically play or stop music depending on the loaded scene — no manual calls needed from other scripts.

- Music toggle state persisted via `PlayerPrefs`
- Music icon in Settings UI updates automatically via `UI_Manager`
- `PlaySFX(AudioClip)` available for one-shot sound effects from any system

---

### 🖥️ UI Manager — `UI_Manager.cs`
Single source of truth for all UI panel state. No other script directly sets panels active or inactive — everything routes through `UI_Manager`.

Panels managed: Main Menu, HUD, Game Over, Pause Menu, Settings, Mobile Controls, Stamina Bar

```csharp
// Clean state transitions — one call sets entire UI context
UI_Manager.Instance.ShowHud();
UI_Manager.Instance.ShowGameOverScreen();
```

---

### ❤️ Health Display — `Health_Display.cs`
Dynamic heart icon system using a property setter pattern. Setting `CurrentHealthPoints` automatically calculates the delta and instantiates or destroys heart icons accordingly — no manual icon management needed externally.

```csharp
// Setting this property auto-adds or removes heart icons
healthDisplay.CurrentHealthPoints = 3; // spawns 3 hearts
healthDisplay.CurrentHealthPoints = 2; // destroys 1 heart
```

---

### 💨 Stamina Bar — `Stamina_bar.cs`
Auto-hides when stamina is full using a smooth `Lerp` on `CanvasGroup.alpha`. The bar only becomes visible when the player is actually spending stamina — clean and non-intrusive UI.

---

### 🏆 Progress Tracker — `Progress_UI.cs` + `Progress_Display.cs`
`ProgressUI` auto-finds all GameObjects tagged `"Coin"` on Start and tracks them via null-check each frame (destroyed coins become null). When the count hits zero, it calls `LevelExit.OpenGate()` automatically.

`Progress_Display` handles the visual — remaining count text and a success panel that swaps in when all coins are collected.

---

### 🪙 Collectable — `Collectable.cs` + `Collectable_animation.cs`
`Collectable` handles trigger detection and fires a `UnityEvent` on collection — completely extensible without touching the script. VFX is called through `VFX_Manager` keeping the collectable itself lightweight.

`Collectable_animation` uses a **sine wave** for a smooth floating effect:

```csharp
float DeltaY = MovementAmplitude * Mathf.Sin(MovementFrequency * Time.time);
CoinMesh.localPosition = new Vector3(x, CoinHeight + DeltaY, z);
```

---

### 🧟 Crawler Zombie — `Crawler_Zombie.cs`
A simpler physics-based enemy using `Rigidbody.MovePosition` in `FixedUpdate` for collision-safe movement that prevents phasing through walls. Rotation is handled separately in `Update` for visual smoothness. Deals damage on collision via `IDamageable`.

---

### 🧍 Player Controller — `Player.cs`
Camera-relative movement using Unity's **New Input System**. Move direction is calculated relative to the camera's forward and right vectors — so the joystick always feels correct regardless of camera angle.

```csharp
// Camera-relative direction so joystick feels natural at any camera angle
Vector3 camForward = Camera.main.transform.forward;
Vector3 camRight = Camera.main.transform.right;
camForward.y = 0f; camForward.Normalize();
camRight.y = 0f;   camRight.Normalize();
return (camForward * moveInput3D.z + camRight * moveInput3D.x);
```

Key features:
- **Sprint** consumes stamina per frame via `PlayerStamina` — auto-cancels when stamina is depleted
- **Jump** uses physics formula `√(2 × h × |g × gravityScale|)` for predictable jump height regardless of gravity scale
- **Landing** fires a `UnityEvent` and triggers Land VFX through `VFX_Manager` — Player doesn't know what reacts
- **Rotation** uses `Quaternion.RotateTowards` for smooth directional turning with a configurable turn speed
- `InputHandling` flag disables all input cleanly on death without removing components

---

### ❤️ Player Health — `Player_Health.cs`
Implements `IDamageable`. Uses a **property setter** to detect the alive → dead transition and fire the `Died` UnityEvent automatically — keeping death logic in one place.

```csharp
// Property setter detects death transition
set {
    bool wasAlive = CurrentHealthPoints > 0;
    maxHealthPoints = Mathf.Max(value, 0);
    if (wasAlive && CurrentHealthPoints <= 0)
    {
        Died.Invoke();                          // → triggers death animation, disables input
        UI_Manager.Instance.ShowGameOverScreen(); // → UI reacts via event
    }
}
```

- **Fall detection** — if player Y drops below -2, `Damage(CurrentHealthPoints)` is called to instantly kill — clean instant death without special-casing
- `Damaged` UnityEvent fires on every hit — drives blink effect, SFX, screen shake all independently

---

### 💪 Player Stamina — `PlayerStamina.cs`
Self-contained stamina pool with **regen delay**. After consuming stamina, regeneration is blocked for `regenDelay` seconds using a time comparison — no coroutines needed.

```csharp
public void ConsumeStamina(float stamina)
{
    points -= stamina;
    allowRegenTime = Time.time + regenDelay; // blocks regen for X seconds
}
```

- `FillAmount` property (`Points / MaxPoints`) feeds directly into the stamina bar UI each frame
- `HasEnoughStamina(float)` lets `Player.cs` check before consuming — no over-spending possible

---

### ✨ Blink — `Blink.cs`
Invincibility blink effect with configurable on/off durations. Call `ActiveBlink(duration)` from anywhere — the script handles the rest independently using time comparisons, no coroutines.

```csharp
// Called from Damaged UnityEvent — Player_Health doesn't know what blinks
blink.ActiveBlink(1.5f);
```

- Targets a `GameObject[]` array — can blink multiple meshes simultaneously
- Automatically restores visibility when duration ends
- Fully decoupled — hooked up via UnityEvent in Inspector, not by code reference

---

### 🎬 Player Animation — `Player_Animation.cs`
Reads state from `Player` and drives the Animator — completely separated from movement logic. Player.cs has zero Animator references.

| Animator Parameter | Driven By |
|---|---|
| `Speed` | `characterController.velocity.magnitude` (Y excluded) |
| `Jump` | `verticalVelocity > 0 && !Grounded` |
| `Fall` | `verticalVelocity < 0 && !Grounded` |
| `Alive` | Set to `false` by `OnDeath()` — called via Died UnityEvent |

`OnDeath()` also sets `player.InputHandling = false` — one call disables both animation and input together.

---

### 🔊 Player SFX — `Player_Sfx.cs`
Lightweight SFX bridge. Holds audio clip references and routes them through `AudioManager.Instance.PlaySFX()`. Called via UnityEvents (e.g. `jumped` event on `Player.cs`) — Player has no AudioManager dependency.

```csharp
// Hooked to Player.jumped UnityEvent in Inspector
public void PlayJumpSound()
{
    AudioManager.Instance.PlaySFX(jumpSound);
}
```

---

## 🔗 Event Flow — How It All Connects

The full player feedback chain showing how systems stay decoupled:

```
Player takes damage
    └── Player_Health.Damage()
            ├── Damaged.Invoke()
            │       ├── Blink.ActiveBlink()         → mesh blinks
            │       ├── PlayerSFX.PlayHitSound()    → SFX plays
            │       └── CinemachineImpulse          → screen shake
            └── (if dead) Died.Invoke()
                    ├── Player_Animation.OnDeath()  → death anim + input disabled
                    └── UI_Manager.ShowGameOver()   → game over screen

Player lands
    └── Player.Landed.Invoke()
            ├── VFX_Manager.PlayLandVFX()           → dust particle
            └── PlayerSFX.PlayLandSound()           → land SFX

Player jumps
    └── Player.jumped.Invoke()
            └── PlayerSFX.PlayJumpSound()           → jump SFX
```

Every arrow is a **UnityEvent** — zero hard dependencies between systems.

---

## 🛠️ Technical Details

| Detail | Value |
|---|---|
| Engine | Unity (URP) |
| Language | C# |
| Target Platform | Android (Mobile) |
| Target Frame Rate | 60 FPS |
| Input System | Unity New Input System (Mobile Joystick) |
| AI Navigation | Unity NavMesh |
| Camera | Cinemachine |
| Rendering Pipeline | Universal Render Pipeline (URP) |

---

## 📦 Build & Run

### Requirements
- Unity 2022.3 LTS or above
- Android Build Support module
- URP package
- Cinemachine package
- TextMeshPro package

### Steps
1. Clone the repo
```bash
git clone https://github.com/sairamyedida06/virus-escape.git
```
2. Open in Unity Hub → select correct Unity version
3. Open `Assets/Scenes/Main Menu` scene
4. Hit **Play** to test in editor, or switch platform to **Android** and build

---

## 🎯 Design Decisions

**Why camera-relative movement?**
With a fixed top-down camera, axis-aligned movement works fine. But if the camera ever rotates or tilts, axis-aligned movement breaks immediately. Camera-relative movement costs nothing extra and makes the control scheme future-proof.

**Why UnityEvents for jump, land, and damage instead of direct calls?**
`Player.cs` and `Player_Health.cs` have zero knowledge of SFX, VFX, animation, or screen shake. Each system subscribes independently in the Inspector. Adding a new reaction to any event — like a controller rumble — requires no code changes anywhere.

**Why a flag for InputHandling instead of disabling the component?**
Disabling a MonoBehaviour stops all its callbacks including things that might still need to run. A bool flag gives precise control — input is blocked but the component stays active for anything else that might need it.


Placing the initializer in each scene makes scenes self-sufficient. Any scene can be opened directly in the editor during development and the managers will be there — no additive loading or boot scenes required.

**Why spawn-anchor the enemy detection radius?**
If detection used the enemy's current position, a roaming enemy could accidentally "discover" the player while patrolling far from its home area. Anchoring to spawn position makes enemy territory predictable and fair.

**Why two trap types on one script?**
Pressure and looping traps share the same FSM states and mesh animation — only the transition logic differs. One script with a `TrapType` enum avoids duplicating all that shared logic across two files.

**Why IDamageable instead of direct component references?**
Any future damageable object — breakable crates, other enemies, destructibles — just implements the interface. No damage dealer needs to be updated. Open for extension, closed for modification.

**Why centralise VFX in a manager?**
If a VFX prefab changes, there is one place to update it. Individual scripts don't hold prefab references, keeping the Inspector clean and the dependency graph shallow.

---

## 👤 Author

**Yedida Sai Ram**
Unity Game Developer
- GitHub: [github.com/sairamyedida06](https://github.com/sairamyedida06)
- Email: sairamyedidaoffl@gmail.com
  

---

## 📄 License

This project is open for viewing and learning purposes.
For commercial use or redistribution, please contact the author.
