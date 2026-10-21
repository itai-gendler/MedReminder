---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the case in Conflicts and click Dismiss
    kind: actor
    actor: administrator
    entities:
      - entity: sync-conflict
        effect: reads
        facts:
          - Medicine
          - Kept value
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product removes the case
    kind: product
    actor: administrator
    entities:
      - entity: sync-conflict
        effect: removes
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

# Accept the kept value

## Trigger

The kept value is right.

## Outcome

The case leaves the list and the data stays as it is.
