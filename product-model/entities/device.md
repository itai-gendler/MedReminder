---
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Sync/SyncPeer.cs#SyncPeer
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdProjection.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Device

A PC (or phone) taking part in a sync group or in the shared installation.

## Information kept

- **Name** — the name given when it joined
- **Platform** — the system it runs on
- **Version** — the MedReminder version it runs
- **Last seen** — when it last synced
- **Role** — Master or Elected master, in the shared installation
- **Profiles held** — the profiles an administrator gave it
