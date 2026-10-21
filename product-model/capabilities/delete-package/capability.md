---
availability:
  - place: desktop::profile
domain: packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#DeleteStockPackage
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Delete a package

Remove a box entered by mistake; the stock does not change.
