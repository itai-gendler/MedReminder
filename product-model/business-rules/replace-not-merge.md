---
appliesTo:
  - type: capability
    id: restore-backup
  - type: capability
    id: import-from-export
  - type: capability
    id: restore-from-cloud-folder
  - type: capability
    id: join-sync-group
  - type: capability
    id: rebuild-from-group
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Persistence/ProfileDatabaseSwap.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Restoring and importing replace a profile's data

A restore, an import, joining a sync group and rebuilding from it replace the profile's data after keeping a copy of it; nothing is merged.
