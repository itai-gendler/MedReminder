---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Enable sync…, choose OneDrive and sign in in the browser window
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
  - text: Enter a name for this PC and a sync passphrase of at least 10 characters, twice
    kind: actor
    actor: administrator
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - This device
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product creates the sync group with this PC as its first device
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: creates
        to: Syncing
        facts:
          - Storage
          - This device
          - Sync passphrase
          - Generation
      - entity: device
        with: sync-group
        effect: creates
        facts:
          - Name
          - Platform
          - Version
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

# Enable sync in OneDrive

## Trigger

An administrator wants the same profile on two PCs.

## Outcome

The profile has a sync group in MedReminder's own OneDrive app folder, encrypted; the other PCs join it with the same account and passphrase.

## Edge cases

- The sync passphrase is not the backup passphrase, and it cannot be recovered.
- With a shared folder, the folder is chosen instead of signing in.
