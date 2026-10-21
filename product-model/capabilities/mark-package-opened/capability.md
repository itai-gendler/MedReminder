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
    target: src/MedReminder.Domain/Stock/PackageAllocation.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Mark a package opened

Record that a box was opened today, so its period after opening starts and it is the one used first.
