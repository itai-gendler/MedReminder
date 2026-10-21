---
availability:
  - place: desktop::administration
domain: sync
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
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncGroupChoiceDialog.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncPairingCodeDialog.cs
---

# Join a sync group

Make this PC take part in a profile's sync group. The profile's data on this PC is replaced by the group's, after a copy.
