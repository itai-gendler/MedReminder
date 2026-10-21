---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Choose the kind Other without a description and save
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
        place: desktop::profile::deadline
  - text: The Product asks for a description
    kind: product
    actor: user
    entities: []
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

# Refuse an Other deadline without description

## Trigger

The person chooses the kind Other and leaves the description empty.

## Outcome

Nothing is recorded until a description is entered.
