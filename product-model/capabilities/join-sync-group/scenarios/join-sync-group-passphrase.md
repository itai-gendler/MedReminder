---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Join a group…, choose the same storage, enter a name for this PC and the passphrase
    kind: actor
    actor: administrator
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Storage
          - This device
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product finds the groups the passphrase opens in the storage
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
  - text: Confirm that the data on this PC is replaced
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product keeps a copy, replaces the profile's data with the group's, joins the group and restarts
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
        effect: changes
        facts:
          - Name
          - Recipient
          - Caregiver e-mail
          - Doctor e-mail
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

# Join with the sync passphrase

## Trigger

An administrator sets up the second PC.

## Outcome

This PC holds the group's data for the profile and MedReminder restarts; from then on changes travel within minutes.

## Decision points

### Which group

How many groups does the passphrase open?

- One → the PC joins it
- Several, one per profile synced with the same passphrase → the administrator chooses the group of this profile, each shown with its devices

## Edge cases

- With a shared folder, it is fully downloaded on the new PC first; with OneDrive or Google Drive the join may take a minute.
