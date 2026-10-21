---
kind: edge
routes:
  desktop: Desktop
steps:
  - text: The installation syncs
    kind: condition
    entities:
      - entity: installation
        effect: reads
        facts:
          - Storage
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: The installation key was changed on another device
    kind: condition
    entities:
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product stops syncing the installation and asks for the new key
    kind: product
    entities:
      - entity: installation
        effect: changes
        from: Published
        to: Needs new key
        facts: []
    contexts:
      desktop:
        place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/HouseholdHostedService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdProjection.cs
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

# Stop when the installation key changed

## Trigger

A device was removed on another device.

## Outcome

This device neither sends nor receives until an administrator enters the new key.
