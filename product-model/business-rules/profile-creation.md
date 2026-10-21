---
appliesTo:
  - type: entity
    id: profile
    effect: creates
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#CreateProfile
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator creates a profile

Profiles are created from Manage profiles, which only administrators see, and the first profile of a PC is an administrator.
