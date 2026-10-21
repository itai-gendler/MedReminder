---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Choose the day and the status Skipped, then register
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Schedule
    contexts:
      desktop:
        place: desktop::profile::register-intake
  - text: The Product records the intake and deducts nothing for that day
    kind: product
    actor: user
    entities:
      - entity: intake
        effect: creates
        facts:
          - Day
          - Quantity
          - Status
          - Notes
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::register-intake
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/RegisterIntake.cs#RegisterIntake
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/IntakeDialog.cs#IntakeDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Register a skipped day

## Trigger

The person did not take the medicine on a day.

## Outcome

Nothing is deducted for that day.

## Edge cases

- Cancelled works the same way: the day gets no automatic deduction and no stock leaves.
