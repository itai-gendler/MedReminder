---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Import from export… and choose the file
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: The Product shows what the export contains
    kind: product
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::import-from-export
  - text: >-
      Type the passphrase, tick I understand that this will overwrite the current profile's data, and click
      Import
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::import-from-export
  - text: >-
      The Product keeps a safety copy, replaces the open profile's medicines and history with the exported
      ones and asks to restart
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
        place: desktop::profile::import-from-export
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Export/ImportService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ImportDialog.cs#ImportDialog
  - kind: doc
    role: intent
    target: docs/EXPORT-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Import an export on a new PC

## Trigger

The person brought an export from the old PC.

## Outcome

The open profile has the exported data and MedReminder restarts; the previous data is kept as a safety copy.
