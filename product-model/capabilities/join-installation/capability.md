---
availability:
  - place: desktop::profile
  - place: desktop::administration
domain: installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/JoinInstallation.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Household/HouseholdProfileInstaller.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/FirstRunWizardForm.cs#FirstRunWizardForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdSync.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HouseholdDialog.cs#HouseholdDialog
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Join an installation

Make this PC a device of an installation used on another PC: it receives the profiles an administrator gives it and the installation's settings.
