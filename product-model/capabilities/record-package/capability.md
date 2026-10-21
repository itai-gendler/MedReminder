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
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackageEditDialog.cs#PackageEditDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record a package

Record a box already in the cabinet, with its expiry, without changing the stock.
