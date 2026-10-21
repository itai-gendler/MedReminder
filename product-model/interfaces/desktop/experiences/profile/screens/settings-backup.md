---
entities:
  - entity: backup-settings
    shows:
      - Daily automatic backup
      - Backup folder
      - Preferred time
      - Retention
      - Cloud backup
      - Storage
      - Snapshots to keep
    collects:
      - Daily automatic backup
      - Backup folder
      - Preferred time
      - Retention
      - Cloud backup
      - Storage
      - Snapshots to keep
      - Backup passphrase
  - entity: profile
    shows:
      - Name
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Backup.cs
---

# Backup / Restore

The Backup / Restore section of Settings: for administrators the daily and cloud backup settings; for every profile the manual copies, the encrypted export and import, and restore from a cloud folder.
