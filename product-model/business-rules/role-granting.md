---
appliesTo:
  - type: entity
    id: administrator
    effect: creates
  - type: entity
    id: user
    effect: creates
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#ChangeProfileRole
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator gives a profile a role

A new profile gets its role from an administrator, the user role by default, and only an administrator makes another profile an administrator.
