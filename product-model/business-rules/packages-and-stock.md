---
appliesTo:
  - type: capability
    id: record-package
  - type: capability
    id: mark-package-opened
  - type: capability
    id: mark-package-finished
  - type: capability
    id: delete-package
  - type: capability
    id: discard-package
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/StockPackage.cs#StockPackage
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#DiscardStockPackage
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only discarding a box changes the stock

Recording, editing, opening, finishing or deleting a box leaves the stock as it is; discarding removes what was left in it, and a discard is final.
