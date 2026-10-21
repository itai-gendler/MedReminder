---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the deadline and click Done
    kind: actor
    actor: user
    entities:
      - entity: deadline
        effect: reads
        facts:
          - Kind
          - Date
    contexts:
      desktop:
        place: desktop::profile::deadlines
  - text: The Product records today as the done date
    kind: product
    actor: user
    entities:
      - entity: deadline
        effect: changes
        from: Due soon
        to: Done
        facts:
          - Done on
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

# Close a one-off deadline

## Trigger

The check-up took place.

## Outcome

The deadline is Done and gives no more reminders.
