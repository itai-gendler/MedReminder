---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Click Add a device…
    kind: actor
    actor: administrator
    entities:
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: No profile on this device takes part in sync
    kind: condition
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Storage
      - entity: profile
        effect: reads
        facts: []
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: The Product asks to enable sync for a profile first
    kind: product
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdPairingOffers.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/Remote/HouseholdPairing.cs
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

# Refuse a profile without sync

## Trigger

No profile of this device is synced.

## Outcome

Nothing can be given: sync is enabled for a profile first.
