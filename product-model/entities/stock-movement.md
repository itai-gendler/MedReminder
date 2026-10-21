---
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/StockMovement.cs#StockMovement
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/StockMovementKind.cs#StockMovementKind
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Stock movement

A change to a medicine's stock that the person records: the initial stock, a new package, a manual addition, or a positive or negative correction. Movements are never edited; a mistake is fixed with a new movement or by deleting the entry from the history.

## Information kept

- **Type** — Initial stock, New package, Manual addition, Positive correction or Negative correction
- **Quantity** — how much it adds or removes
- **Day** — when it happened
- **Notes** — optional
