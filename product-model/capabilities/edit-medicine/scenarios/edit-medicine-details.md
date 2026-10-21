---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Double-click the medicine, or select it and press F2
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Change the warning threshold, the notification channels or Remind me at dose time
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Warning threshold
          - Notification channels
          - Remind at dose time
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
  - text: The Product saves the fields that changed
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Warning threshold
          - Notification channels
          - Remind at dose time
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

# Change how a medicine warns

## Trigger

The person wants an earlier warning, another channel or a reminder at dose time.

## Outcome

Only the fields the person changed are saved, so a value another device changed meanwhile is not overwritten.

## Edge cases

- Remind me at dose time is available only when the medicine has a slot with a time and some stock; otherwise it is saved off.
- Changed administration times are a new set from today; earlier days keep the slots they had.
