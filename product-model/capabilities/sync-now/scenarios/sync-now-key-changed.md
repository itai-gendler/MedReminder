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
          - Storage
    contexts:
      desktop:
        place: desktop::administration
    unattended: true
  - text: The group key was changed on another device
    kind: condition
    entities:
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration
  - text: The Product stops sending and asks for the new key
    kind: product
    entities:
      - entity: sync-group
        effect: changes
        from: Syncing
        to: Needs new key
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

# Stop when the group key changed elsewhere

## Trigger

Another device changed the group key.

## Outcome

This device sends nothing until it gets the new key; its changes are kept.
