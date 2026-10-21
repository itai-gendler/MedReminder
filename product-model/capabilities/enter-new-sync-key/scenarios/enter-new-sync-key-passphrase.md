---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Enter the new key… and type the new passphrase, or use a pairing code from a device that has it
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
  - text: >-
      The Product takes the new key, rebuilds the profile from the group keeping this PC's changes, and
      restarts
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: changes
        from: Needs new key
        to: Syncing
        facts:
          - Sync passphrase
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::sync
references:
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

# Enter the new sync passphrase

## Trigger

The status says the group key was changed on another device.

## Outcome

This PC syncs again: the profile is rebuilt from the group, its own changes are kept and sent afterwards, and MedReminder restarts.

## Edge cases

- Changes about a medicine the group no longer has are not kept, and the person is told how many.
