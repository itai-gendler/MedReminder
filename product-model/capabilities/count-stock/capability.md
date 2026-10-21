---
availability:
  - place: desktop::profile
domain: medicines
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

# Count stock

Count what is in the cabinet and let MedReminder record one correction so that the stock equals the count.
