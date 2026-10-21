---
availability:
  - place: desktop::administration
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Startup.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/AutoStart/RegistryAutoStartService.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change Startup settings

Start MedReminder hidden in the notification area at Windows login, so reminders arrive without opening it. No administrator rights on Windows are needed.
