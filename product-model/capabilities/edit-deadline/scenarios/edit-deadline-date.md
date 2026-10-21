---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the deadline and click Edit…
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
  - text: Change the date and save
    kind: actor
    actor: user
    entities:
      - entity: deadline
        effect: reads
        facts:
          - Date
    contexts:
      desktop:
        place: desktop::profile::deadline
  - text: The Product saves the deadline
    kind: product
    actor: user
    entities:
      - entity: deadline
        effect: changes
        facts:
          - Date
    contexts:
      desktop:
        place: desktop::profile::deadline
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

# Move a deadline to the new date

## Trigger

The documents show a different date.

## Outcome

The deadline has the new date and its reminder follows it.

## Edge cases

- A recurring deadline cannot be closed with a done date: Done moves it to its next date instead.
