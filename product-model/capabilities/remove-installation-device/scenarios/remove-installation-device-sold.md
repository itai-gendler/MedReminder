---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: On the master, open Devices, select the device and click Remove device…
    kind: actor
    actor: administrator
    entities:
      - entity: device
        effect: reads
        facts:
          - Name
          - Profiles held
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: Choose to change the installation passphrase and type a new one, or keep the current one
    kind: actor
    actor: administrator
    entities:
      - entity: installation
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: The Product removes the device and moves the installation and its profiles to new keys
    kind: product
    actor: administrator
    entities:
      - entity: device
        effect: removes
      - entity: installation
        effect: changes
        facts:
          - Installation passphrase
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: The Product offers to show a code to enter on each other device
    kind: product
    actor: administrator
    entities:
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/RemoveDevice.cs
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

# Remove a sold PC

## Trigger

A device of the installation was sold.

## Outcome

The removed device keeps what it has but receives nothing new; the other devices stop syncing until they get the new key.

## Edge cases

- Removing the master from another device makes that device the master.
- A device that does not hold every profile of the removed device cannot change their keys: the removal is done on the master.
