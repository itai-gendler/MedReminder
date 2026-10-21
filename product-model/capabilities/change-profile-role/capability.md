---
availability:
  - place: desktop::administration
domain: profiles
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#ChangeProfileRole
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change role

Make a profile an administrator or a user. At least one administrator always remains, and the open profile's role cannot change.
