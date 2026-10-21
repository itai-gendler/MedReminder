---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Restore from cloud folder…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: Choose the folder or account and pick a copy by date, profile and PC
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::restore-from-cloud
  - text: Type the backup passphrase, tick the confirmation and click Restore
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::restore-from-cloud
  - text: The Product replaces the open profile's medicines and history with the copy's and restarts
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Name
          - Active ingredient
          - Package
          - Product code
          - Unit
          - Schedule
          - Administration times
          - Therapy start date
          - Therapy end date
          - Warning threshold
          - Notification channels
          - Remind at dose time
          - Reference doctor
          - Notes
          - Stock
          - Days left
          - Run-out date
          - Status
          - Supply
          - Expiry
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::restore-from-cloud
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Export/CloudRestoreService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/RestoreFromCloudDialog.cs#RestoreFromCloudDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Restore a cloud copy

## Trigger

The person lost their PC and installs MedReminder on a new one.

## Outcome

The open profile has the data of the chosen copy and MedReminder restarts; this is a backup, not sync.

## Edge cases

- To restore another profile, that profile is opened first.
- Changes made on two PCs between restores are not merged.
