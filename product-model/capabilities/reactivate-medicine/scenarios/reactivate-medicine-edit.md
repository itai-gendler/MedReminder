---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Show inactive medicines and open the medicine with Edit
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Tick Active and save
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: The Product marks the medicine active from today
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        from: Inactive
        to: Active
        facts:
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/UpdateMedicine.cs#UpdateMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/MedicineActivity.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Reactivate an inactive medicine

## Trigger

The person takes a stopped medicine again.

## Outcome

The medicine is active again from today; nothing was deducted for the days it was inactive.
