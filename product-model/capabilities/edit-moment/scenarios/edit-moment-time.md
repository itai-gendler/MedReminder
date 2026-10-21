---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the moment, click Edit…, change the time and save the dose times
    kind: actor
    actor: user
    entities:
      - entity: moment
        effect: reads
        facts:
          - Name
          - Time
    contexts:
      desktop:
        place: desktop::profile::dose-times
  - text: The Product saves the new time for this PC
    kind: product
    actor: user
    entities:
      - entity: moment
        effect: changes
        facts:
          - Time
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

# Change the time of a moment

## Trigger

Breakfast is later than 08:00.

## Outcome

Doses placed at that moment are counted at the new time during the day; the stock recorded does not change.

## Edge cases

- The name of a shipped moment cannot be changed.
