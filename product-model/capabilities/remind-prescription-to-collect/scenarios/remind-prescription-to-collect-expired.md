---
kind: edge
routes:
  desktop: Desktop
steps:
  - text: A new day starts
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: >-
      Valid until has passed and the prescription was not collected (or a repeatable one still has
      dispensations)
    kind: condition
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Valid until
          - Collected on
      - entity: dispensation
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product shows the prescription as expired
    kind: product
    entities:
      - entity: prescription
        effect: changes
        from: To collect
        to: Expired
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionReminders.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/RepeatablePrescriptionNotices.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/PrescriptionRules.cs#PrescriptionRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A prescription lapses uncollected

## Trigger

The last valid day of a prescription to collect passes.

## Outcome

The prescription shows Expired in the list.
