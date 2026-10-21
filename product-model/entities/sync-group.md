---
relations:
  - entity: device
    verb: includes
    cardinality: one-to-many
  - entity: sync-conflict
    verb: has
    cardinality: one-to-many
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncGroupUseCases.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Sync/JsonSyncSettingsStore.cs
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Sync group

The end-to-end encrypted group that keeps one profile identical on several devices, kept in OneDrive, Google Drive or a shared folder. Medicines, stock, intakes, prescriptions, deadlines, the profile name and the recipients travel through it.

## Information kept

- **Storage** — OneDrive, Google Drive or a shared folder every device reaches
- **This device** — the name this PC has in the group
- **Sync passphrase** — at least 10 characters; it is not the backup passphrase and cannot be recovered
- **Generation** — restarts when an import or a restore replaces the data
- **Last sync** — when this device last synced, with changes sent and received

## States

### Syncing

This device sends and receives changes.

### Needs new key

The group key was changed on another device; this device sends nothing until it gets the new key.

### Needs rebuild

Another device started a new generation, or this device was away too long.
