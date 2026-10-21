---
appliesTo:
  - type: capability
    id: restore-backup
  - type: capability
    id: import-from-export
  - type: capability
    id: restore-from-cloud-folder
  - type: entity
    id: sync-group
    facts:
      - Generation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncGenesis.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Replacing a synced profile's data starts a new generation

After an import or a restore on a synced profile, the other devices are warned, discard their changes not sent yet and rebuild from the group.
