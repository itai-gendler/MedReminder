---
availability:
  - place: desktop::administration
domain: sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Sync/ProfileSyncRebuild.cs
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

# Rebuild from the group

Replace the profile's data on this PC with the group's, after another device started a new generation or this device was away too long.
