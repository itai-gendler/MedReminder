---
appliesTo:
  - type: entity
    id: administrator
    effect: removes
  - type: entity
    id: user
    effect: removes
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

# Only an administrator takes a role away

Changing or deleting a profile ends its role; only an administrator does either.
