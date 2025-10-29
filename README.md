# Homicide Hoover ![Unity](https://img.shields.io/badge/Engine-Unity-20232A?logo=unity&logoColor=white) ![C#](https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp&logoColor=white)

You are the **Homicide Hoover**, a tiny automated vacuum forced to **clean up bloody crime scenes** before the police arrive. Vacuum up evidence and maintain your structural integrity while dodging furniture to achieve the perfect five-star clean.

---

*“You were built by a shady cleaning company that “specialises in crime scene recovery”, but one night… the blood just keeps coming.”*

---

## 📝 Details

| Item | Info |
|------|------|
| **Engine** | Unity |
|**Editor Version** | 6000.2.10f1 |
| **Language** | C# |
| **Platform** | PC (Windows), |
| **Assets** | 3D models / stylized lighting |
| **Genre** | Casual Cleaning Simulator / Arcade Survival (Comedic Dark Humour) |
| **Theme** | Unlikely Hero / Hiding in Plain Sight / Crime Scene Clean-Up |
| **Build Status** | Pre-Alpha — Prototype in development |

---

## 🎮 Core Loop (The Three C's)

> “**Collect:** The player vacuums items (blood, debris, weapons) to earn points and fill the trash bag.
> **Clog/Penalty:** Hitting furniture or “enemies” fills the **Clog Meter**. Hitting the limit breaks the Roomba (movement stops, a time penalty occurs). 
> **Contain:** When the bag is full, the player must return to a designated drop zone (e.g., the front door) to dispose of the trash bag and start fresh.”

---

## ⚙️ Mechanics

| Mechanic | Detail | Game Integration |
|------|------|------|
| Player Movement | WASD controls the Roomba. Movement is slower when the Clog Meter is high or the bag is nearly full. | Implemented via Mover.cs |
| Vacuum Action | Auto-collects items when the collider passes over them (OnTriggerEnter). | Implemented via OnTriggerEnter in Mover.cs and DustCollection.cs |
| The Clog Meter | Hits/Penalty: Hitting furniture increases the hitLimit (Max 3 hits before shutdown). | Implemented via OnCollisionEnter and BreakVacuumSequence Coroutine. |
| Trash Bag Capacity | Max Capacity: 30 units. Items collected add units (see Section 2.3). | Requires a new maxCapacity variable and a bag drop zone mechanic. |
| Bag Drop | When currentCapacity >= maxCapacity, a “Drop Bag” prompt appears near the designated zone. Dropping the bag resets capacity to 0, granting bonus points, and spawns a physical “Trash Bag” object to be avoided. | Requires: Raycast/trigger for Drop Zone, and a new Trash Bag Prefab. |

---

## 📸 Screenshots / GIFs

> _(Coming soon)_

---

## 📝 License

This project is currently private and under development. Licensing will be updated closer to release.

---

## 💬 Feedback & Contributions

Feedback, testing, and creative ideas are always welcome.  
If you'd like to contribute feedback, please reach out!

---
