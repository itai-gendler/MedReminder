---
singleton: true
relations:
  - entity: device
    verb: includes
    cardinality: one-to-many
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdSync.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Household/MasterRules.cs
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Shared installation

Everything around the profiles that every device of a family shares: the profiles with their names, roles and PINs, the e-mail account, the cloud backup policy and the reference country. One device, the master, sends every e-mail and makes the cloud backup.

## Information kept

- **Storage** — the same storage that holds the profiles' sync groups
- **Installation passphrase** — lets an administrator add a device and recover every profile when no other device is at hand; it cannot be recovered
- **Master device** — the device that sends e-mail and runs the cloud backup

## States

### Not published

MedReminder is used on this device only.

### Published

The installation is shared and syncs every 15 minutes.

### Needs new key

A device was removed elsewhere; this device neither sends nor receives until an administrator enters the new key.
