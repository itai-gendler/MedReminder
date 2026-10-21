---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DoseTimesDialog.cs#DoseTimesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/DoseTimes/DoseTimeSettings.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change the dose times of medicines without slots

Set when the doses of a medicine without administration times fall in the day, by number of administrations per day (1 a day = 08:00, 2 a day = 08:00 and 20:00, …).
