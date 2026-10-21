---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Join with a pairing code… and type the code shown on the other PC
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: >-
      The Product joins the group the code belongs to, replacing the profile's data with the group's, and
      restarts
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: creates
        to: Syncing
        facts:
          - Storage
          - This device
          - Sync passphrase
          - Generation
      - entity: device
        with: sync-group
        effect: creates
        facts:
          - Name
          - Platform
          - Version
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncGroupUseCases.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncDialog.cs#SyncDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Sync/SyncSetupService.cs
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncGroupChoiceDialog.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncPairingCodeDialog.cs
---

# Join with a pairing code

## Trigger

A PC already in the group shows a pairing code.

## Outcome

This PC joins without typing the passphrase.

## Edge cases

- A code that expired, was replaced by a newer one or belongs to another group is refused.
