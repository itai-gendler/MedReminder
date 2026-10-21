---
appliesTo:
  - type: entity
    id: sync-conflict
    effect: removes
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncConflictActions.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator resolves sync conflicts

Conflicts are listed in the Sync window, which only administrators reach.
