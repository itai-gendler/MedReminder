---
availability:
  - place: desktop::profile
domain: packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#SaveStockPackage
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Mark a package finished

Record that a box was used up, so it leaves the cabinet.
