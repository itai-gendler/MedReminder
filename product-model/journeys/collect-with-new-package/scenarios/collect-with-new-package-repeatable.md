---
kind: alternate
result: achieved
routes:
  desktop: Desktop
steps:
  - text: Add the purchase to the medicine's stock and save
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Stock
    contexts:
      desktop:
        place: desktop::profile::stock-movement
    capability: add-package
  - text: The Product records the purchase as a stock movement
    kind: product
    actor: user
    entities:
      - entity: stock-movement
        effect: creates
        facts:
          - Type
          - Quantity
          - Day
          - Notes
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::stock-movement
    capability: add-package
  - text: The Product shows how many dispensations are collected and asks whether to record one today
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Issued on
          - Dispensations allowed
      - entity: dispensation
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
    capability: add-package
  - text: Answer yes
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
    capability: record-dispensation
  - text: The Product records a dispensation today, with the number of boxes bought
    kind: product
    actor: user
    entities:
      - entity: dispensation
        effect: creates
        facts:
          - Collected on
          - Packages
    contexts:
      desktop:
        place: desktop::profile::medicines
    capability: record-dispensation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#CollectPrescription
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record a dispensation after adding the package

## Trigger

The person adds a new package of a medicine with a repeatable prescription that has dispensations left.

## Outcome

The stock is refilled and one more dispensation is recorded today.
