---
appliesTo:
  - type: entity
    id: profile
    effect: changes
    facts:
      - PIN
permits:
  - actors:
      - administrator
  - related:
      - verb: uses
        entity: user
  - unattended: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#SetProfilePin
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A person changes their own PIN; an administrator changes any PIN

Settings → Notifications → My PIN changes the open profile's own PIN; Manage profiles → Change PIN changes or clears any profile's PIN. The shared installation brings PIN changes made on another device.
