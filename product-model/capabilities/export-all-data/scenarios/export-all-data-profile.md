---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Export all data (encrypted)…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::settings-backup
  - text: >-
      Choose the file, type a passphrase of at least 12 characters twice and, optionally, include the SMTP
      password and the backup and user settings
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::export-all-data
  - text: Click Export
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::export-all-data
  - text: The Product writes the encrypted export of the profile
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::export-all-data
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Export/ExportService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ExportDialog.cs#ExportDialog
  - kind: doc
    role: intent
    target: docs/EXPORT-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Export the open profile

## Trigger

The person moves to a new PC.

## Outcome

One .mrz file holds the profile's data, readable only with the passphrase.

## Edge cases

- The passphrase cannot be recovered: without it the file can never be read again.
- An included SMTP password is encrypted with the passphrase.
