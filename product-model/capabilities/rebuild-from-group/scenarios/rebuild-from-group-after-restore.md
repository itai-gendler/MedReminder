---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Rebuild from the group… and confirm
    kind: actor
    actor: administrator
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Generation
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product keeps a copy, replaces the profile's medicines and history with the group's and restarts
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: changes
        from: Needs rebuild
        to: Syncing
        facts:
          - Generation
      - entity: medicine
        effect: changes
        facts:
          - Name
          - Active ingredient
          - Package
          - Product code
          - Unit
          - Schedule
          - Administration times
          - Therapy start date
          - Therapy end date
          - Warning threshold
          - Notification channels
          - Remind at dose time
          - Reference doctor
          - Notes
          - Stock
          - Days left
          - Run-out date
          - Status
          - Supply
          - Expiry
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Sync/ProfileSyncRebuild.cs
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

# Rebuild after a restore elsewhere

## Trigger

The status says the profile must be rebuilt from the group.

## Outcome

This PC has the group's data again and syncs; changes not sent yet are lost, and a copy of the previous data is kept.
