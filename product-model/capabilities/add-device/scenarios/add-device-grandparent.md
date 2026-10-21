---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Open the Devices section and click Add a device…
    kind: actor
    actor: administrator
    entities:
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: Tick the profiles for the new device and click Show the code
    kind: actor
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: The Product shows the code for those profiles
    kind: product
    actor: administrator
    entities:
      - entity: device
        effect: reads
        facts:
          - Profiles held
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

# Add a grandparent's PC with only their profile

## Trigger

A grandparent's PC should hold only the grandparent's profile.

## Outcome

A code valid for 10 minutes, or until its window closes, lets that PC join with only the ticked profiles.
