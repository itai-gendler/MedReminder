---
appliesTo:
  - type: entity
    id: device
    effect: creates
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdPairingOffers.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/JoinInstallation.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator adds a device

Devices join a sync group or the installation from the administrators' windows; joining with the installation passphrase also needs an administrator's PIN.
