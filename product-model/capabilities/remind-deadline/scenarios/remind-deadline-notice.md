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
  - text: A deadline not done reached Remind days before its date and was not reminded for that date
    kind: condition
    entities:
      - entity: deadline
        effect: reads
        facts:
          - Date
          - Remind days before
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product reminds of the deadline through its channels
    kind: product
    entities:
      - entity: deadline
        effect: changes
        from: Upcoming
        to: Due soon
        facts: []
    contexts:
      desktop:
        place: desktop::profile
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

# Remind when the notice period starts

## Trigger

A deadline enters its notice period.

## Outcome

One reminder comes through the deadline's own channels, and the deadline is due soon.

## Edge cases

- Clicking the notification opens the deadlines.
