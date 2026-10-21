---
kind: edge
routes:
  desktop: Desktop
steps:
  - text: The master checks when it last synced the installation
    kind: condition
    entities:
      - entity: installation
        effect: reads
        facts:
          - Master device
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: More than 24 hours passed since its last sync
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product stops sending e-mail from this device and says so in the Installation status
    kind: product
    entities:
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

# Stop e-mail from a master out of touch

## Trigger

The master device has not synced the installation for 24 hours.

## Outcome

The master sends no e-mail until it syncs again, so a stale master cannot double the e-mails of a new one.
