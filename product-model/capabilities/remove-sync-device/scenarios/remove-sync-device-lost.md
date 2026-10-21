---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Open the Devices section, select the PC and click Remove device…
    kind: actor
    actor: administrator
    entities:
      - entity: device
        effect: reads
        facts:
          - Name
          - Last seen
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: Confirm and choose a new sync passphrase, not the current one
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product removes the PC and moves the group to a new key
    kind: product
    actor: administrator
    entities:
      - entity: device
        effect: removes
      - entity: sync-group
        effect: changes
        facts:
          - Sync passphrase
    contexts:
      desktop:
        place: desktop::administration::sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncKeyRotation.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Sync/ProfileSyncRekey.cs
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
---

# Remove a lost PC from the group

## Trigger

A PC of the group was lost or stolen.

## Outcome

The removed PC reads nothing written from now on; every other device stops syncing until it gets the new key.

## Edge cases

- What the removed PC already holds stays readable to it.
- The lost PC should also be signed out in the Microsoft or Google account.
