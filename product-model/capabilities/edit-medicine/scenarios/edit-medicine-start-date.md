---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Change the therapy start date and save
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Therapy start date
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: >-
      The Product makes the schedule and administration times follow the new start date and derives the
      deducted doses again
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Therapy start date
          - Stock
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

# Move the therapy start date

## Trigger

The therapy actually started on another day.

## Outcome

The schedule and administration times follow the new start date, and the stock is derived again from it.
