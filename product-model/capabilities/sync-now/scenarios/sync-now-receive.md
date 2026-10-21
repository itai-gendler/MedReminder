---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Sync runs by itself, every 5 minutes or 10 seconds after a change on this PC
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
  - text: The Product applies the changes of the other devices, among them a therapy suspension
    kind: product
    entities:
      - entity: suspension
        effect: creates
        to: Ongoing
        facts:
          - From
          - To
          - Reason
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
          - Status
      - entity: sync-group
        effect: changes
        facts:
          - Last sync
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

# Receive changes made on another device

## Trigger

Another device of the group recorded intakes, a suspension or a new prescription.

## Outcome

They appear on this PC within minutes, and the stock and run-out date follow them.

## Edge cases

- Every device of a group runs the same MedReminder version: newer operation schemas stop older apps.
