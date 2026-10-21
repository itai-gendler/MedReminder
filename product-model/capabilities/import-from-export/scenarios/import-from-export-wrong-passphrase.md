---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Type the passphrase and click Import
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::import-from-export
  - text: The file cannot be opened with the passphrase, is damaged, or is from a newer version
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile::import-from-export
  - text: The Product stops and leaves the profile's data as it was
    kind: product
    actor: user
    entities:
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

# Stop an import that cannot be read

## Trigger

The passphrase is wrong, the file is damaged, or it comes from a newer version.

## Outcome

The import stops without touching the data.
