---
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/StockCount.cs#StockCount
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ReconcileStock.cs#ReconcileStock
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Stock count

What the person counted in the cabinet on a day. MedReminder records one correction so that the stock equals the count; the difference is a stock figure, never read as missed or extra doses.

## Information kept

- **Day** — the day of the count
- **Counted quantity** — what was in the cabinet
- **Already taken today** — what had already been taken that day when counting
- **Correction** — the difference recorded so that the stock equals the count
- **Notes** — optional
