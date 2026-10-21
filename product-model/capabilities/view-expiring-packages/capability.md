---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/ExpiringPackagesQuery.cs#ExpiringPackagesQuery
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ExpiringPackagesDialog.cs#ExpiringPackagesDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# View expiring packages

Gather the expired and expiring boxes of every medicine of the profile, including medicines no longer in use, and open the boxes of one of them.
