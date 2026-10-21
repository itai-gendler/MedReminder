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

# Change key and passphrase

Replace the group key and the sync passphrase, for example when the passphrase leaked.
