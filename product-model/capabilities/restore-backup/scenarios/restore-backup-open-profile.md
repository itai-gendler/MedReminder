---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Restore backup… and choose a .db copy
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: Choose the profile that receives it (the one the copy came from is proposed) and confirm
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: >-
      The Product keeps the current data aside, puts the copy's medicines and history in its place and
      restarts on the profile
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

# Restore a copy into the open profile

## Trigger

The data looks damaged, or a mistake cannot be undone otherwise.

## Outcome

The profile has the data of the copy; the previous data is kept aside with the date, and MedReminder restarts.

## Edge cases

- On a synced profile the restore starts a new generation: the other devices are warned and must rebuild from the group.
- Restoring into a profile that is not open needs no restart.
