---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click New… in the list of deadlines
    kind: actor
    actor: user
    entities:
      - entity: deadline
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::deadlines
  - text: >-
      Choose the kind Exemption renewal, the date printed on the documents, Remind days before, Repeat every
      12 months and the channels, then save
    kind: actor
    actor: user
    entities:
      - entity: deadline
        effect: reads
        facts:
          - Kind
          - Date
          - Remind days before
          - Repeat every
          - Notify by
    contexts:
      desktop:
        place: desktop::profile::deadline
  - text: The Product records the deadline for the profile
    kind: product
    actor: user
    entities:
      - entity: deadline
        effect: creates
        to: Upcoming
        facts:
          - Kind
          - Description
          - Medicine
          - Date
          - Remind days before
          - Repeat every
          - Notify by
      - entity: profile
        effect: reads
        facts: []
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

# Add a yearly exemption renewal

## Trigger

The person's exemption must be renewed every year.

## Outcome

The deadline is listed as Upcoming and its reminder starts the chosen number of days before the date.

## Edge cases

- MedReminder has no rule of its own for these dates: validity periods differ by plan and region.
- Remind days before is 14 by default, up to 180; a recurring deadline repeats every 1 to 120 months.
