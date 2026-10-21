---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Therapy → Dose times…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Click Add…, type the name and, if it has one, the time; save the dose times
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
  - text: The Product records the moment, shown in the slot window
    kind: product
    actor: user
    entities:
      - entity: moment
        effect: creates
        facts:
          - Name
          - Time
          - As needed
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
    target: src/MedReminder.Application/DoseTimes/DoseTimeSettings.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Add an own moment of the day

## Trigger

None of the shipped moments fits, for example "After the walk".

## Outcome

The moment is offered in the slot window with its time.
