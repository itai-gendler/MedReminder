---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Type exactly two times for 2 a day and save the dose times
    kind: actor
    actor: user
    entities:
      - entity: preferences
        effect: reads
        facts:
          - Default dose times
    contexts:
      desktop:
        place: desktop::profile::dose-times
  - text: The Product saves the times for this PC
    kind: product
    actor: user
    entities:
      - entity: preferences
        effect: changes
        facts:
          - Default dose times
    contexts:
      desktop:
        place: desktop::profile::dose-times
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DoseTimesDialog.cs#DoseTimesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/DoseTimes/DoseTimeSettings.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change the times for two doses a day

## Trigger

The person takes their twice-a-day medicines at 07:00 and 19:00.

## Outcome

During the day, those doses leave the stock shown at the new times.

## Edge cases

- A row with a different number of times than its administrations is refused.
