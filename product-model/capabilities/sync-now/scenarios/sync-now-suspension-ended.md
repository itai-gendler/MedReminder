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
  - text: The Product applies the end date recorded on the other device
    kind: product
    entities:
      - entity: suspension
        effect: changes
        from: Ongoing
        to: Ended
        facts:
          - To
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
          - Status
      - entity: device
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

# Receive the end of a suspension

## Trigger

The therapy was resumed on another device of the group.

## Outcome

The suspension has its end date on this PC, and the medicine is deducted and warns again from the day after.
