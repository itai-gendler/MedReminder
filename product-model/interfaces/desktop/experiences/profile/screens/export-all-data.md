---
entities:
  - entity: profile
    shows:
      - Name
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ExportDialog.cs#ExportDialog
---

# Export window

Writes one encrypted file with all the data of a profile, protected by a passphrase.
