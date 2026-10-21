---
availability:
  - place: desktop::administration
domain: sync
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

# Dismiss a conflict

Accept the value a sync conflict kept and remove the case from the list.
