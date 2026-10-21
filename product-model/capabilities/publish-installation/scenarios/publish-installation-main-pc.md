---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Publish the installation…, choose the storage already used for sync and an installation passphrase
    kind: actor
    actor: administrator
    entities:
      - entity: installation
        effect: reads
        facts:
          - Storage
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: The Product publishes the installation with this PC as master
    kind: product
    actor: administrator
    entities:
      - entity: installation
        effect: changes
        from: Not published
        to: Published
        facts:
          - Storage
          - Installation passphrase
          - Master device
      - entity: device
        effect: creates
        facts:
          - Name
          - Role
          - Profiles held
    contexts:
      desktop:
        place: desktop::administration::installation
references:
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

# Publish from the main PC

## Trigger

An administrator wants the family set-up on several PCs, with one PC sending the e-mail.

## Outcome

The installation is shared and this PC is its master; other devices can be added.

## Edge cases

- The installation passphrase is kept for administrators only and cannot be recovered.
