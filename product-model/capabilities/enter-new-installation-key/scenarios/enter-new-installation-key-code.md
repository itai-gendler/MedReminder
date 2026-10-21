---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Enter the new key…, choose With a code and type the code shown on a device that has the new key
    kind: actor
    actor: administrator
    entities:
      - entity: installation
        effect: reads
        facts: []
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: The Product takes the new key and syncs the installation again
    kind: product
    actor: administrator
    entities:
      - entity: installation
        effect: changes
        from: Needs new key
        to: Published
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdKeyring.cs
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

# Enter the new key with a code

## Trigger

The Installation window says the key changed on another device.

## Outcome

The device syncs the installation again; changes made there meanwhile are kept.
