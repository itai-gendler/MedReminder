---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Open the Conflicts section, select the case and click Restore lost value
    kind: actor
    actor: administrator
    entities:
      - entity: sync-conflict
        effect: reads
        facts:
          - Medicine
          - Field
          - Kept value
          - Lost value
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: >-
      The Product writes the lost value back as a new change, which every device then keeps, and clears the
      case
    kind: product
    actor: administrator
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Name
          - Active ingredient
          - Package
          - Product code
          - Unit
          - Therapy end date
          - Warning threshold
          - Notification channels
          - Remind at dose time
          - Reference doctor
          - Notes
      - entity: sync-conflict
        effect: removes
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncConflictActions.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncDialog.cs#SyncDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Restore the value another device overwrote

## Trigger

The value that lost a conflict was the right one.

## Outcome

The lost value is back, on every device after the next sync, and the case leaves the list.

## Edge cases

- Only a field of a medicine edited on two devices can be restored; the other cases, such as two schedule changes for the same date, can only be dismissed.
