---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Select the recurring deadline and click Done
    kind: actor
    actor: user
    entities:
      - entity: deadline
        effect: reads
        facts:
          - Date
          - Repeat every
    contexts:
      desktop:
        place: desktop::profile::deadlines
  - text: The Product moves the date forward by the recurrence, counted from the previous date
    kind: product
    actor: user
    entities:
      - entity: deadline
        effect: changes
        from: Overdue
        to: Upcoming
        facts:
          - Date
    contexts:
      desktop:
        place: desktop::profile::deadlines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Deadlines/DeadlineUseCases.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DeadlinesDialog.cs#DeadlinesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DeadlineEditDialog.cs#DeadlineEditDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Deadlines/DeadlineRules.cs#DeadlineRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Move a recurring deadline to its next date

## Trigger

The person renewed a plan that recurs every 12 months, a few days late.

## Outcome

The deadline moves to the same date one recurrence later and is Upcoming again.
