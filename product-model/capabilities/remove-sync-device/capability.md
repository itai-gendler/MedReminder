---
availability:
  - place: desktop::administration
domain: sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncKeyRotation.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Sync/ProfileSyncRekey.cs
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

# Remove a sync device

Remove a lost or stolen PC from a profile's group: a new group key and passphrase replace the old ones, so it reads nothing written from then on.
