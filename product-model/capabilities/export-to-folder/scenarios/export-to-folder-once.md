---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Export to specific folder… and choose the folder
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: The Product copies the open profile's database into the folder
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
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

# Copy the database to a USB stick

## Trigger

The person wants a copy outside the PC.

## Outcome

A copy of the open profile's database is in the chosen folder.
