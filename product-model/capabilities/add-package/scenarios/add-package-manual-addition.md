---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Choose the type Manual addition, enter the quantity and save
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
  - text: The Product records the manual addition
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
    target: src/MedReminder.Application/UseCases/AddStock.cs#AddStock
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/StockAdjustmentDialog.cs#StockAdjustmentDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Add samples as a manual addition

## Trigger

The person received some units without buying a package, for example samples from the doctor.

## Outcome

The stock grows and the warning cycle starts again.
