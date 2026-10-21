---
appliesTo:
  - type: capability
    id: count-stock
  - type: entity
    id: stock-count
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ReconcileStock.cs#ReconcileStock
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A stock count is a stock figure, not a dose record

The difference between the count and the expected stock is recorded as one correction; it is never read as missed or extra doses.
