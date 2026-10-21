---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine and choose Stock → Count stock…
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Type the counted quantity
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Stock
    contexts:
      desktop:
        place: desktop::profile::stock-count
  - text: >-
      The Product shows the expected stock, the difference and how the run-out date changes, and suggests as
      already taken today the doses whose time has passed
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Stock
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::stock-count
  - text: Adjust what was already taken today and click Record count
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::stock-count
  - text: The Product records the count and one correction so that the stock equals it
    kind: product
    actor: user
    entities:
      - entity: stock-count
        effect: creates
        facts:
          - Day
          - Counted quantity
          - Already taken today
          - Correction
          - Notes
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::stock-count
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ReconcileStock.cs#ReconcileStock
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/StockCountDialog.cs#StockCountDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Ledger/LedgerDeriver.cs#LedgerDeriver
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record a stock count

## Trigger

The count in the cabinet does not match the app.

## Outcome

The stock equals what was counted, and the run-out date follows it.

## Edge cases

- When the count matches the expected stock, nothing is recorded.
- A count that raises the stock restarts the warning cycle only when the days left end up above the warning threshold.
