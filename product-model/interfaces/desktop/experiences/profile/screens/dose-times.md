---
entities:
  - entity: moment
    shows:
      - Name
      - Time
      - As needed
      - Shown
    collects:
      - Name
      - Time
      - As needed
      - Shown
  - entity: preferences
    shows:
      - Default dose times
    collects:
      - Default dose times
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DoseTimesDialog.cs#DoseTimesDialog
---

# Dose times

The moments of the day with their time, and the times used for medicines without slots.
