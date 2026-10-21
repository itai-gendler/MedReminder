---
availability:
  - place: desktop::profile
domain: packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#SaveStockPackage
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Edit a package

Correct a box's quantity, expiry, period after opening, opening date or batch.
