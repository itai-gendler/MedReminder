---
availability:
  - place: desktop::profile
  - place: desktop::administration
domain: profiles
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#CreateProfile
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/FirstRunWizardForm.cs#FirstRunWizardForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Profiles/ProfileRegistry.cs#ProfileRegistry
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Create a profile

Add a person to the installation, with a name, a role and an optional PIN. The first profile on a PC is created from the welcome window and is an administrator.
