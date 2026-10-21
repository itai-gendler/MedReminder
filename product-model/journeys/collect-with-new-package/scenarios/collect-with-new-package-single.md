---
kind: primary
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
  - text: The Product finds a prescription of the medicine to collect and asks whether the purchase came from it
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Issued on
      - entity: medicine
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
    capability: mark-prescription-collected
  - text: The Product records the prescription as collected today
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: changes
        from: To collect
        to: Collected
        facts:
          - Collected on
    contexts:
      desktop:
        place: desktop::profile::medicines
    capability: mark-prescription-collected
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

# Mark the prescription collected after adding the package

## Trigger

The person adds a new package of a medicine that has a prescription to collect.

## Outcome

The stock is refilled and the prescription is Collected today.
