---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Change the regime or switch between Simple and Advanced
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Schedule
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: Save
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: The Product saves the other fields and a new schedule version together, from today or the start date
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Schedule
    contexts:
      desktop:
        place: desktop::profile::medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/EditMedicine.cs#EditMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/UpdateMedicine.cs#UpdateMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/TherapyStartChange.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change the regime while editing

## Trigger

The person opens a medicine on an advanced regime and changes the regime in the same window.

## Outcome

The new regime applies from today, or from the start date when that is later; the days before keep the old schedule.
