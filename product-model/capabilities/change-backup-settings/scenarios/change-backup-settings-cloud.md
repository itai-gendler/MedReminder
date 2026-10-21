---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Tick Backup to a cloud-synced folder (encrypted), choose OneDrive and click Sign in…
    kind: actor
    actor: administrator
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Cloud backup
          - Storage
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: >-
      Sign in with the Microsoft account, choose the snapshots to keep and set a backup passphrase of at least
      12 characters
    kind: actor
    actor: administrator
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Snapshots to keep
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: Save
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: >-
      The Product saves the cloud backup; with a shared installation, the storage and snapshots to keep reach
      every device
    kind: product
    actor: administrator
    entities:
      - entity: backup-settings
        effect: changes
        facts:
          - Cloud backup
          - Storage
          - Snapshots to keep
          - Backup passphrase
      - entity: device
        effect: reads
        facts: []
      - entity: installation
        effect: reads
        facts: []
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

# Back up to OneDrive, encrypted

## Trigger

An administrator wants a copy that survives the loss of the PC.

## Outcome

An encrypted copy of every profile goes to MedReminder's app folder in OneDrive every day; the passphrase stays on this PC.

## Edge cases

- Losing the backup passphrase means losing the copies.
- Whoever knows the passphrase can read every profile's copy.
- The copies do not contain the SMTP password or the preferences.
