---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Clear Active and save
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: The Product marks the medicine inactive from today
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        from: Active
        to: Inactive
        facts:
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/DeactivateMedicine.cs#DeactivateMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/MedicineActivity.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Deactivate a medicine while editing it

## Trigger

The person clears Active in the medicine window.

## Outcome

The medicine is inactive from today, like a deactivation from the menu.
