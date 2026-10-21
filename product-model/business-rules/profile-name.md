---
appliesTo:
  - type: entity
    id: profile
    effect: changes
    facts:
      - Name
permits:
  - actors:
      - administrator
  - unattended: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator renames a profile

Names change from Manage profiles, the guided setup of an administrator, or the shared installation bringing a rename made on another device.
