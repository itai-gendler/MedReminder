---
availability:
  - place: desktop::administration
domain: sync
references:
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

# Enter the new sync key

Give this PC the group's new key, with the new passphrase or a pairing code, after it was changed on another device.
