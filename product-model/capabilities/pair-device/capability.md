---
availability:
  - place: desktop::administration
domain: sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/Remote/SyncPairing.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncPairingDialog.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Pair a device

Show a pairing code on a PC already in the group, so another device joins or takes the new key without the passphrase.
