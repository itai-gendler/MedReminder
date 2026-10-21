---
availability:
  - place: desktop::profile
domain: backup
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Export/CloudRestoreService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/RestoreFromCloudDialog.cs#RestoreFromCloudDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Restore from cloud folder

Restore one of the encrypted cloud copies into the open profile, for example on a second PC.
