---
appliesTo:
  - type: capability
    id: rename-profile
  - type: entity
    id: sync-group
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs#SyncedProfileRenameException
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A synced profile is renamed while it is open

The name of a synced profile travels with its own data, so another profile that takes part in sync is opened before renaming it.
