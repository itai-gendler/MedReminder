---
availability:
  - place: desktop::profile
  - place: desktop::administration
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#SetProfilePin
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ChangePinDialog.cs#ChangePinDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change PIN

Set, change or clear a profile's PIN: a person their own, an administrator any profile's.
