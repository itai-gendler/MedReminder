---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Clear Shown for the moment and save the dose times
    kind: actor
    actor: user
    entities:
      - entity: moment
        effect: reads
        facts:
          - Name
          - Shown
    contexts:
      desktop:
        place: desktop::profile::dose-times
  - text: The Product hides the moment from the slot window
    kind: product
    actor: user
    entities:
      - entity: moment
        effect: changes
        facts:
          - Shown
    contexts:
      desktop:
        place: desktop::profile::dose-times
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DoseTimesDialog.cs#DoseTimesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/DoseTimePreset.cs#DoseTimePreset
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Hide a moment not used

## Trigger

The person never uses a shipped moment.

## Outcome

The moment is no longer offered in the slot window.
