---
appliesTo:
  - type: capability
    id: pair-device
  - type: capability
    id: add-device
  - type: capability
    id: join-sync-group
  - type: capability
    id: join-installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncKeyRotation.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A pairing or device code works for 10 minutes, while its window is open

Whoever sees a code while it is valid can read the group's data, so it is shown only when the other device is ready and withdrawn when its window closes.
