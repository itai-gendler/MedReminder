---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateGeneralSettings
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.General.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Settings/ProfileUiSettingsFile.cs#ProfileUiSettingsFile
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change General settings

Choose the interface language, this profile's text size and appearance and the update check; an administrator also chooses the reference country and database-query logging.
