---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The preferred time passes while MedReminder runs, or it starts later that day
    kind: condition
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Daily automatic backup
          - Preferred time
    contexts:
      desktop:
        place: desktop::profile::settings-backup
    unattended: true
  - text: >-
      The Product copies every profile's database into the backup folder and deletes copies older than the
      retention
    kind: product
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Backup folder
          - Retention
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
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

# Make the daily copies

## Trigger

The preferred time arrives, or MedReminder starts later that day after the PC was off.

## Outcome

Each profile has a copy named after it with the date and time; copies older than the retention are deleted, except the newest of each profile.
