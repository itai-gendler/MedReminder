---
appliesTo:
  - type: capability
    id: change-dose-and-frequency
  - type: capability
    id: edit-medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ChangeMedicationSchedule.cs#ChangeMedicationSchedule
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A schedule change applies from its date

Every change of dose, frequency or regime is a new version from its Effective from date; the days before keep the old schedule, and when the date is in the past the doses already deducted from it are recalculated.
