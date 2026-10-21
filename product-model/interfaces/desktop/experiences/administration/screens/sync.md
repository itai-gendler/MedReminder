---
entities:
  - entity: sync-group
    shows:
      - Storage
      - This device
      - Generation
      - Last sync
    collects:
      - Storage
      - This device
      - Sync passphrase
  - entity: device
    shows:
      - Name
      - Platform
      - Version
      - Last seen
  - entity: sync-conflict
    shows:
      - Detected
      - Medicine
      - Case
      - Field
      - Kept value
      - Lost value
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncDialog.cs#SyncDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncPairingDialog.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncPassphraseDialog.cs
---

# Sync between devices

The open profile's sync group: its status (storage, this device, generation, last sync), its devices, and the conflicts found.
