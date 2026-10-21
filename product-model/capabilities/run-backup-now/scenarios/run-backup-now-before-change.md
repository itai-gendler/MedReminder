---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Run backup now
    kind: actor
    actor: administrator
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Backup folder
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: The Product copies every profile's database into the backup folder
    kind: product
    actor: administrator
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Backup folder
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
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

# Copy every profile now

## Trigger

An administrator is about to change something important.

## Outcome

Every profile is copied into the backup folder at once.
