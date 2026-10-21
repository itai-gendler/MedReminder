---
availability:
  - place: desktop::profile
domain: backup
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Export/ImportService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ImportDialog.cs#ImportDialog
  - kind: doc
    role: intent
    target: docs/EXPORT-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Import from export

Replace the open profile's data with an encrypted export, after a safety copy.
