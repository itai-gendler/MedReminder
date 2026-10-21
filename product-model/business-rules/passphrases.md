---
appliesTo:
  - type: capability
    id: export-all-data
  - type: capability
    id: change-backup-settings
  - type: capability
    id: enable-sync
  - type: capability
    id: publish-installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Export/ExportFormat.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncPassphraseDialog.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Passphrases cannot be recovered

Export and cloud backup passphrases have at least 12 characters, the sync passphrase at least 10; the installation passphrase is kept for administrators. None can be recovered: files encrypted with a lost one stay unreadable, though new ones can be set.
