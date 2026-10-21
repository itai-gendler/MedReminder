---
appliesTo:
  - type: entity
    id: device
    effect: changes
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdMasterRole.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/MasterHandover.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator moves the master role

Make master… and the handover are offered to administrators only.
