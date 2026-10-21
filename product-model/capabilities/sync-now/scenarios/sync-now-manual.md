---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Sync now
    kind: actor
    actor: administrator
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Last sync
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: >-
      The Product sends this PC's changes, applies the group's, and shows how many changes it sent and
      received
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: changes
        facts:
          - Last sync
    contexts:
      desktop:
        place: desktop::administration::sync
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

# Sync on request

## Trigger

An administrator wants the other PC's latest changes now.

## Outcome

The status shows the last run with the changes sent and received.
