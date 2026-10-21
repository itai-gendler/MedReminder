---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The installation syncs, every 15 minutes
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
  - text: The Product applies the changes made on the other devices
    kind: product
    entities:
      - entity: profile
        effect: changes
        facts:
          - Name
          - PIN
      - entity: email-account
        effect: changes
        facts:
          - Host
          - Port
          - Use StartTLS
          - Username
          - Password
          - Sender
          - Timeout
      - entity: backup-settings
        effect: changes
        facts:
          - Storage
          - Snapshots to keep
      - entity: preferences
        effect: changes
        facts:
          - Reference country
      - entity: device
        effect: reads
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

# Bring the other devices' changes

## Trigger

An administrator changed a PIN or the e-mail account on another device.

## Outcome

The change is on this device within 15 minutes.

## Edge cases

- A profile created later is given to other devices with Add a device on a device that holds it, after enabling its sync.
