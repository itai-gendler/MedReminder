---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Open Devices, select the new device, click Make master… and confirm
    kind: actor
    actor: administrator
    entities:
      - entity: device
        effect: reads
        facts:
          - Name
          - Role
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: The Product elects the device master; no device sends e-mail meanwhile
    kind: product
    actor: administrator
    entities:
      - entity: device
        effect: changes
        facts:
          - Role
    contexts:
      desktop:
        place: desktop::administration::installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdMasterRole.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Household/MasterRules.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdSync.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HouseholdDialog.cs#HouseholdDialog
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Plan a handover to another PC

## Trigger

Another PC is switched on more often.

## Outcome

The chosen device is the elected master; it takes over once the handover is completed there and the current master hands over at its next sync.
