---
availability:
  - place: desktop::profile
domain: backup
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/AutomaticBackupHostedService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Backup/CloudBackupRun.cs
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

# Back up every profile daily

Copy every profile once a day to the backup folder, and write the encrypted cloud copy, keeping as many as the settings say.
