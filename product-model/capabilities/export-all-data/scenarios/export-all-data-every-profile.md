---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: >-
      Tick Export every profile (one encrypted file per profile), choose a folder and the passphrase, and
      export
    kind: actor
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::export-all-data
  - text: The Product writes one encrypted export per profile
    kind: product
    actor: administrator
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

# Export every profile

## Trigger

An administrator with several profiles moves the whole installation.

## Outcome

One encrypted file per profile is written into the chosen folder.
