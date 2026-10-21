---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine whose day differs from the schedule
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Choose the day, enter the quantity and keep the status Taken
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
  - text: Register
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::register-intake
  - text: The Product records the intake and replaces the day's automatic deduction with its quantity
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

# Register a dose taken differently

## Trigger

On one day the person took a different quantity from the schedule.

## Outcome

That day's automatic deduction is replaced by the quantity registered, and the stock goes down at once.
