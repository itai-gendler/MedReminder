---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Enter the new dose with an Effective from date in the past and save
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Schedule
    contexts:
      desktop:
        place: desktop::profile::change-dose-frequency
  - text: The Product adds the schedule version and recalculates the deducted doses from that date
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Schedule
          - Stock
          - Days left
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::change-dose-frequency
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

# Correct the dose from a past date

## Trigger

The therapy changed some days ago and the person records it late.

## Outcome

The consumption already deducted from that date on is recalculated with the new schedule.
