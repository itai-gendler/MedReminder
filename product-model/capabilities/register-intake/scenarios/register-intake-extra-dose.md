---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Tick Extra dose as needed, enter the quantity and register
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
  - text: The Product records the extra intake and deducts it on top of the day's schedule
    kind: product
    actor: user
    entities:
      - entity: intake
        effect: creates
        facts:
          - Day
          - Quantity
          - Status
          - Extra dose
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

# Register an extra dose as needed

## Trigger

The person took a dose on top of the schedule, for example a painkiller.

## Outcome

The extra quantity is deducted and the day's scheduled doses still count.

## Edge cases

- The option appears only for medicines with a schedule, and is already ticked when the medicine has an as-needed slot.
