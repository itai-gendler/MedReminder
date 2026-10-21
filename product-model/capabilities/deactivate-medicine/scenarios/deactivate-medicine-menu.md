---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine and choose Therapy → Deactivate, then confirm
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product marks the medicine inactive from today and hides it from the list
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
        place: desktop::profile::medicines
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

# Deactivate a medicine from the menu

## Trigger

The person stops taking a medicine.

## Outcome

The medicine is inactive: hidden from the list, not consumed and silent, with its history kept.
