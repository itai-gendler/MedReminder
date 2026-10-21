---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Select a shipped moment
    kind: actor
    actor: user
    entities:
      - entity: moment
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::dose-times
  - text: The Product keeps Remove unavailable for it
    kind: product
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::dose-times
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DoseTimesDialog.cs#DoseTimesDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Keep a shipped moment

## Trigger

The person selects a moment MedReminder ships.

## Outcome

Remove is unavailable; the moment can be hidden instead.
