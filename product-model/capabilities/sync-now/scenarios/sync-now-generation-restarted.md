---
kind: edge
routes:
  desktop: Desktop
steps:
  - text: Sync runs
    kind: condition
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Generation
    contexts:
      desktop:
        place: desktop::administration
    unattended: true
  - text: The group started a new generation
    kind: condition
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Generation
    contexts:
      desktop:
        place: desktop::administration
  - text: The Product asks to rebuild the profile from the group
    kind: product
    entities:
      - entity: sync-group
        effect: changes
        from: Syncing
        to: Needs rebuild
        facts: []
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncEngine.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/ApplyRemoteOperations.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/SyncHostedService.cs
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

# Ask to rebuild after a new generation

## Trigger

An import or a restore on another device started a new generation, or this device was away too long.

## Outcome

The profile must be rebuilt from the group.
