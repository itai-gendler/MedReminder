---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine and choose Stock → Correct stock…
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Keep the type Negative correction, enter the quantity to subtract and save
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
  - text: The Product records the negative correction
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
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/AdjustStockDown.cs#AdjustStockDown
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/StockAdjustmentDialog.cs#StockAdjustmentDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Subtract lost units

## Trigger

The person has less than MedReminder shows.

## Outcome

The stock goes down by the quantity entered; the warning cycle is not restarted.
