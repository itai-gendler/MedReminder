---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Disable sync… and confirm
    kind: actor
    actor: administrator
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Storage
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product leaves the group on this PC and keeps the data
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: removes
        from: Syncing
    contexts:
      desktop:
        place: desktop::administration::sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncGroupUseCases.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncDialog.cs#SyncDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Sync/SyncSetupService.cs
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Stop syncing on this PC

## Trigger

This PC should no longer follow the profile's group.

## Outcome

This PC keeps its data and no longer syncs; the other devices are unaffected.
