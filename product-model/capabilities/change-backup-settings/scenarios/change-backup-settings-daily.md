---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Tools → Settings… and the Backup / Restore section
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Tick Daily automatic backup, choose the backup folder, the preferred time and the retention, and save
    kind: actor
    actor: administrator
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Daily automatic backup
          - Backup folder
          - Preferred time
          - Retention
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: The Product saves the backup settings of this PC
    kind: product
    actor: administrator
    entities:
      - entity: backup-settings
        effect: changes
        facts:
          - Daily automatic backup
          - Backup folder
          - Preferred time
          - Retention
    contexts:
      desktop:
        place: desktop::profile::settings-backup
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateBackupSettings
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

# Turn on the daily backup

## Trigger

An administrator wants a copy of every profile every day.

## Outcome

Every profile is copied every day at the preferred time into the chosen folder, ideally an external disk.

## Edge cases

- MedReminder warns against a cloud-synced folder for this backup: its copies are not encrypted.
