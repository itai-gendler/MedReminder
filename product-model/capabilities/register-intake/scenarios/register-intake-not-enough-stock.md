---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Register a taken quantity larger than the stock
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
  - text: The intake would push the stock below zero
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Stock
      - entity: intake
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::register-intake
  - text: The Product refuses the intake
    kind: product
    actor: user
    entities:
      - entity: intake
        effect: reads
        facts: []
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

# Refuse an intake larger than the stock

## Trigger

The quantity taken is more than the stock recorded.

## Outcome

Nothing is recorded: the stock never goes below zero.
