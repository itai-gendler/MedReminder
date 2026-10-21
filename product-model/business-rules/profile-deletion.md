---
appliesTo:
  - type: entity
    id: profile
    effect: removes
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#DeleteProfile
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator deletes a profile

An administrator deletes a profile after typing its name; never the open profile, and never the last administrator.
