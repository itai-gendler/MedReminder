---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine just bought and open the stock window
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Keep the type New package and enter the quantity and the number of identical boxes
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Stock
      - entity: package
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::stock-movement
  - text: >-
      Tick the expiry and choose the printed month and year; optionally the days of use after opening, Opened
      today and the batch
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::stock-movement
  - text: Save
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::stock-movement
  - text: The Product records the new package and one box per package bought, splitting the quantity between them
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
      - entity: package
        effect: creates
        to: Unopened
        facts:
          - Quantity
          - Expiry
          - Use within
          - Batch
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

# Add a new package with its expiry

## Trigger

The person bought one or more boxes of a medicine.

## Outcome

The stock grows, the warning cycle starts again, and each box is in the cabinet with its expiry.

## Edge cases

- An expiry of 03/2027 is good until 31 March 2027.
- Opened today opens the first box at once.
- The days of use after opening are proposed from the medicine's latest package.
- With every expiry field empty, the package only adds quantity.
