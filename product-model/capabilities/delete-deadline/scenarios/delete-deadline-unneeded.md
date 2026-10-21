---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the deadline, click Delete and confirm
    kind: actor
    actor: user
    entities:
      - entity: deadline
        effect: reads
        facts:
          - Kind
          - Description
    contexts:
      desktop:
        place: desktop::profile::deadlines
  - text: The Product removes the deadline
    kind: product
    actor: user
    entities:
      - entity: deadline
        effect: removes
        from: Upcoming
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

# Delete a deadline that no longer applies

## Trigger

The deadline no longer applies.

## Outcome

The deadline is gone and gives no reminders.
