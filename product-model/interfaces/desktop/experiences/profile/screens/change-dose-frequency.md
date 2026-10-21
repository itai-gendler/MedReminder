---
entities:
  - entity: medicine
    shows:
      - Name
      - Schedule
    collects:
      - Schedule
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ChangeScheduleDialog.cs#ChangeScheduleDialog
---

# Change dose / frequency

A new dose and frequency, or a new Advanced regime, for one medicine, with the date it applies from.
