---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select a moment the person added, click Remove and save the dose times
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
  - text: The Product removes the moment
    kind: product
    actor: user
    entities:
      - entity: moment
        effect: removes
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

# Remove an own moment

## Trigger

A moment the person added is no longer needed.

## Outcome

The moment is gone from the slot window.
