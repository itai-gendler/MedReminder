---
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/DoseTimePreset.cs#DoseTimePreset
  - kind: code
    role: implementation
    target: src/MedReminder.Application/DoseTimes/DoseTimeSettings.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Moment

A moment of the day that places doses in the day, such as "In the morning" at 08:00 or "Before lunch" at 13:00. MedReminder ships a set of moments and the person can add their own. Moments only place doses; they never change the stock recorded, and they apply to the PC they were set on.

## Information kept

- **Name** — the label shown in the slot window
- **Time** — the time of day, if the moment has one
- **As needed** — slots with this moment are not deducted automatically
- **Shown** — whether the moment is offered in the slot window
