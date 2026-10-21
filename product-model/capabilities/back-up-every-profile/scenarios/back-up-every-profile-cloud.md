---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: The daily backup runs with the cloud backup on
    kind: condition
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Cloud backup
          - Storage
    contexts:
      desktop:
        place: desktop::profile::settings-backup
    unattended: true
  - text: This device is the master, or the installation is not shared
    kind: condition
    entities:
      - entity: installation
        effect: reads
        facts:
          - Master device
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: The Product writes an encrypted snapshot of every profile and keeps the configured number
    kind: product
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Storage
          - Snapshots to keep
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

# Write the encrypted cloud copy

## Trigger

The cloud backup is on.

## Outcome

An encrypted snapshot of every profile is in the cloud storage; with a shared installation only the master device writes it.
