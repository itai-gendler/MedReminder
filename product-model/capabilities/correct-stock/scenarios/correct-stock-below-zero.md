---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Enter a quantity larger than the stock and save
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
  - text: The correction would push the stock below zero
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Stock
    contexts:
      desktop:
        place: desktop::profile::stock-movement
  - text: The Product refuses the correction
    kind: product
    actor: user
    entities: []
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

# Refuse a correction below zero

## Trigger

The quantity to subtract is more than the stock.

## Outcome

Nothing is recorded.
