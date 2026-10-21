---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ChangeMedicationSchedule.cs#ChangeMedicationSchedule
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ChangeScheduleDialog.cs#ChangeScheduleDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change dose/frequency

Record a change of dose, frequency or regime from a date, keeping the old schedule for the days before it.
