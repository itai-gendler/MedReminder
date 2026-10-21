---
availability:
  - place: desktop::profile
domain: packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#DiscardStockPackage
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Discard a package

Record a box thrown away, typically expired: what was left in it leaves the stock. A discard is final.
