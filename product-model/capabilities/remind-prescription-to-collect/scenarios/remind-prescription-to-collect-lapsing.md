---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The periodic check runs
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: A prescription to collect is within 3 days of Valid until and was not reminded for that date
    kind: condition
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Valid until
          - Issued on
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product reminds to collect the prescription through the medicine's channels
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Notification channels
      - entity: prescription
        effect: reads
        facts:
          - Valid until
    contexts:
      desktop:
        place: desktop::profile
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

# Remind before a prescription lapses

## Trigger

An issued prescription is 3 days from its last valid day and still to collect.

## Outcome

One reminder comes through the medicine's channels; it does not include the prescription code.

## Edge cases

- For a repeatable prescription the reminder comes only while dispensations are left, and says how many would be lost.
- Clicking the notification opens the prescriptions.
