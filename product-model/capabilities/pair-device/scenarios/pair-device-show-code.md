---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Pair a device…, then Show the code when the other PC is ready
    kind: actor
    actor: administrator
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Storage
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product shows the pairing code and withdraws it when the window closes or 10 minutes pass
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Storage
    contexts:
      desktop:
        place: desktop::administration::sync
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

# Show a pairing code

## Trigger

The other PC is ready to join.

## Outcome

A code, also as a QR code, works for 10 minutes and only while its window is open.

## Edge cases

- Anyone who sees the code while it is valid can read the group's data: it is shown only to the person's own devices, never sent by e-mail or message.
