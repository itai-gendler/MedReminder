---
appliesTo:
  - type: entity
    id: stock-movement
  - type: capability
    id: add-package
  - type: capability
    id: correct-stock
  - type: capability
    id: count-stock
  - type: capability
    id: delete-history-entry
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/StockMovement.cs#StockMovement
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Ledger/LedgerDeriver.cs#LedgerDeriver
  - kind: doc
    role: intent
    target: README.md
---

# The stock changes only through what is recorded

The stock is what the stock movements add and remove, less the doses deducted day by day, as intakes and counts adjust them. Quantities are never edited: a mistake is fixed with a new movement, a count, or by deleting the entry from the history.
