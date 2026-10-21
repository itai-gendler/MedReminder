---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Change key and passphrase…, confirm and choose a new passphrase
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
  - text: The Product moves the group to a new key and passphrase
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: changes
        facts:
          - Sync passphrase
    contexts:
      desktop:
        place: desktop::administration::sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncKeyRotation.cs
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

# Change the key after a leaked passphrase

## Trigger

The sync passphrase may be known to someone else.

## Outcome

A new key and passphrase protect what is written from now on; the other devices stop until they get them, keeping their changes.
