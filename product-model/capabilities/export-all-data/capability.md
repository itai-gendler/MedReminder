---
availability:
  - place: desktop::profile
domain: backup
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Export/ExportService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ExportDialog.cs#ExportDialog
  - kind: doc
    role: intent
    target: docs/EXPORT-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Export all data

Write all the data of a profile into one encrypted file, protected by a passphrase, to move to a new PC or keep safe. The format is public.
