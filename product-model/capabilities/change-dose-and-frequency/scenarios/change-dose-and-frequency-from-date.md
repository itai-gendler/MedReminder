---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine and choose Therapy → Change dose/frequency…
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Enter the new dose and administrations per day, or an advanced regime, and the Effective from date
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
  - text: Save the change
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::change-dose-frequency
  - text: The Product adds a schedule version from the effective date
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Schedule
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

# Change the dose from a coming date

## Trigger

The doctor changes the therapy.

## Outcome

From the effective date the new dose and frequency are deducted; the days before keep the old ones.

## Edge cases

- The administration times stay as they are; under an advanced regime they only place the day's quantity.
