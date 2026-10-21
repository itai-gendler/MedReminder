---
appliesTo:
  - type: entity
    id: device
    effect: removes
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/RemoveDevice.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator removes a device

Removing a device moves the group or the installation to a new key.
