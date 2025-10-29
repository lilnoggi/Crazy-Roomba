# 🧹 Homicide Hoover 
![Unity](https://img.shields.io/badge/Engine-Unity-20232A?logo=unity&logoColor=white) 
![C#](https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp&logoColor=white)
![Status](https://img.shields.io/badge/Build-Pre--Alpha-orange)

> “You were built by a shady cleaning company that *specialises in crime scene recovery*, but one night... **the blood just keeps coming.**”

---

## 💡 Overview

You are the **Homicide Hoover**, a tiny automated vacuum forced to **clean up bloody crime scenes** before the police arrive.  
Vacuum up evidence, dodge hazards, and maintain your structural integrity to achieve the perfect **Five-Star Clean**.

| Item | Info |
|------|------|
| **Engine** | Unity |
| **Version** | 6000.2.10f1 |
| **Language** | C# |
| **Platform** | PC (Windows) |
| **Genre** | Casual Cleaning Simulator / Arcade Survival |
| **Tone** | Comedic Dark Humour |
| **Status** | Pre-Alpha (Prototype) |

---

## 🎮 Core Loop (The Three C’s)

> **Collect** → **Clog** → **Contain**

1. **Collect:** Vacuum up items (blood, debris, weapons) to earn score and fill your trash bag.  
2. **Clog:** Hitting furniture or enemies fills the **Clog Meter**. Too many hits = system shutdown.  
3. **Contain:** When your bag is full, return to the drop zone to dispose of evidence, reset capacity, and continue cleaning.

---

## ⚙️ Key Mechanics

| Mechanic | Description | Integration |
|-----------|--------------|--------------|
| **Movement** | WASD controls the Hoover. Speed decreases with higher clog or bag capacity. | `Mover.cs` |
| **Vacuum Action** | Auto-collects items via collider triggers. | `OnTriggerEnter` in `Mover.cs` & `DustCollection.cs` |
| **Clog Meter** | Tracks collisions. 3 hits = shutdown and time penalty. | `OnCollisionEnter`, `BreakVacuumSequence()` |
| **Bag Capacity** | Max 30 units. Items increase unit count. | `maxCapacity` variable in progress. |
| **Drop Zone** | Dispose full bags for score bonuses and spawn physical “Trash Bags” to avoid. | Requires `Raycast`/trigger zone. |

---

## 🧼 Items to Clean

| Item | Score | Capacity | Notes |
|------|--------|-----------|-------|
| Dust/Dirt | +10 | 1 | Common debris |
| Bloody Footprints | +25 | 1 | Found across rooms |
| Blood Splatter | +50 | 2 | Near “victim” props |
| Weapon/Evidence | +200 | 5 | Unique per level |

---

## 🚨 Hazards & Enemies

| Hazard | Description | Penalty |
|---------|--------------|----------|
| **Furniture** | Collisions fill Clog Meter. | Minor slowdown |
| **Patrolling Guards** | Move in fixed loops with small FOV. | Police timer drops by -20s when spotted |
| **Mops / Buckets** | Instant clog. | System shutdown |

---

## 🕹️ Progression & Levels

### ⭐ Level 1 – *The Suburban House*  
> Tutorial & Intro  
- Setting: Cozy suburban home with carpet (slow movement)  
- Goal: 3,000 points  
- Drop Zone: Front door mat  
- Hazards: Chairs, rugs  

### 💎 Level 2 – *The High-Roller Hotel Room*  
> Speed & Precision  
- Goal: 5,500 points  
- Hazards: Glass tables, sharp furniture corners  
- Evidence: Diamond ring, shell casing  
- Drop Zone: Laundry hamper  

### 🎰 Level 3 – *The Casino Back Office*  
> Complexity & Stealth  
- Goal: 8,000 points  
- Hazards: Server racks, rolling chairs, patrolling guard  
- Drop Zone: Hidden garbage chute  

---

## 🧭 Ranking System

| Rank | Requirement | Reward |
|------|--------------|--------|
| ⭐⭐⭐⭐⭐ | ≥120% score goal, <3 clogs, 0 dropped bags | Bonus Time Multiplier + unlock next level |
| ⭐⭐⭐⭐ | ≥100% score goal, ≤5 clogs | Unlock next level |
| ⭐⭐⭐ | ≥80% score goal | Base pass |
| ☠ Failure ☠ | Timer reaches 0 or 6+ clogs | Retry level |

---

## 🧩 User Interface

| Element | Location | Function |
|----------|-----------|-----------|
| Police Timer | Top Right | Flashes red under 20s |
| Score / Goal | Top Left | Turns green when goal met |
| Clog Meter | Bottom Right | Tracks structural integrity |
| Capacity Meter | Bottom Left | Warns when full |
| Drop Prompt | Near Drop Zone | Appears at ≥90% capacity |

**Menus:**  
- **Main Menu:** Roomba cleans logo with jazz music. Sticky-note options: *Play*, *Options*, *Quit*.  
- **Pause Menu:** Snapshot freeze with “Scene Contained (For Now).”  
- **Post-Level:** Animated clipboard summary.  
- **Failure:** Sirens, “EVIDENCE COMPROMISED”, Retry option.

---

## 🎨 Art & Audio Direction

| Category | Description |
|-----------|--------------|
| **Art Style** | Low-poly 3D, exaggerated lighting, comedic noir tone |
| **Palette** | Neutral tones with sharp red/blue/yellow accents |
| **Lighting** | Harsh “crime scene” contrast; flickers under pressure |
| **Animation** | Smooth, floaty motion; furniture wiggles when hit |
| **Soundtrack** | Jazz noir meets Katamari — comedic tension music |
| **SFX** | Vacuum pops, alert beeps, collision clunks |
| **Failure Jingle** | Record scratch + muffled siren fade-out |

---

## 🧠 Inspirations

- 🧽 **PowerWash Simulator** – satisfying clean-up loop  
- 💀 **Hotline Miami** – dark comedy tone, tension  
- 🌀 **Untitled Goose Game** – silly physicality, slapstick chaos  

---

## 🧰 Development Notes

- **Scripts:** `Mover.cs`, `DustCollection.cs`, `GameManager.cs`
- **Next Goals:**  
  - Implement Bag Drop Zone logic  
  - Add Clog Meter UI feedback  
  - Prototype Level 1 layout  

---

## 📸 Media

> _(Screenshots, gifs, and trailers coming soon!)_

---

## 📝 License

This project is currently **private and under active development.**  
License terms will be finalised closer to release.

---

## 💬 Feedback & Contributions

Feedback, testing, and creative ideas are always welcome!  
Please open an issue or reach out directly to share suggestions.

---

**“Horror, but make it clean.”**
