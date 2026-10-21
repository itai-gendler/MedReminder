---
kind: alternate
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
  - text: Two devices changed the same field, the same date's schedule or the same day's intakes
    kind: condition
    entities:
      - entity: intake
        effect: reads
        facts: []
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration
  - text: The Product keeps the latest change and lists the case with the kept and lost values
    kind: product
    entities:
      - entity: sync-conflict
        effect: creates
        facts:
          - Detected
          - Medicine
          - Case
          - Field
          - Kept value
          - Lost value
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

# Keep the latest of two concurrent changes

## Trigger

Two devices changed the same thing before syncing.

## Outcome

The latest change wins and the case is listed in Conflicts with the value that was lost.
