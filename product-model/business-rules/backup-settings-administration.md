---
appliesTo:
  - type: entity
    id: backup-settings
    effect: changes
permits:
  - actors:
      - administrator
  - unattended: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateBackupSettings
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator changes the backup settings

For a user the automatic backup settings are hidden; the manual copies, exports, imports and restores stay available to every profile.
