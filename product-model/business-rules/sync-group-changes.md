---
appliesTo:
  - type: entity
    id: sync-group
    effect: changes
permits:
  - actors:
      - administrator
  - unattended: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncKeyRotation.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator changes a sync group

Keys, passphrases and rebuilds are changed by an administrator; the group itself records each sync run.
