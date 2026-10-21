---
availability:
  - place: desktop::profile
domain: backup
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Backup/BackupService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Backup.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Export to specific folder

Make one copy of the open profile's database now, in a folder the person chooses.
