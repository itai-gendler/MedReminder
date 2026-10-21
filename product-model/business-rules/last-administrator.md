---
appliesTo:
  - type: capability
    id: change-profile-role
  - type: capability
    id: delete-profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#ProfileAdministration
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# At least one administrator always remains

The last administrator can neither be made a user nor deleted; the open profile's role cannot change and the open profile cannot be deleted.
