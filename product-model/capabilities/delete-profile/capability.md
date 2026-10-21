---
availability:
  - place: desktop::administration
domain: profiles
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#DeleteProfile
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Delete a profile

Remove a profile, after typing its name to confirm. Its data on disk is kept unless the administrator asks to delete it too.
