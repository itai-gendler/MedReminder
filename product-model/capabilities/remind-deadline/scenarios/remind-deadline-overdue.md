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
  - text: The date of a deadline not done has passed
    kind: condition
    entities:
      - entity: deadline
        effect: reads
        facts:
          - Date
          - Done on
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product shows the deadline as overdue
    kind: product
    entities:
      - entity: deadline
        effect: changes
        from: Due soon
        to: Overdue
        facts: []
    contexts:
      desktop:
        place: desktop::profile::deadlines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Deadlines/DeadlineReminders.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Deadlines/DeadlineRules.cs#DeadlineRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A deadline becomes overdue

## Trigger

The date of a deadline not done passes.

## Outcome

The deadline is shown in red as overdue until it is done.
