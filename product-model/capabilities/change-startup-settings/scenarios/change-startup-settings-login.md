---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Tick Start MedReminder at Windows login
    kind: actor
    actor: administrator
    entities:
      - entity: preferences
        effect: reads
        facts:
          - Start at Windows login
    contexts:
      desktop:
        place: desktop::administration::settings-startup
  - text: The Product registers the start at login for this Windows account
    kind: product
    actor: administrator
    entities:
      - entity: preferences
        effect: changes
        facts:
          - Start at Windows login
    contexts:
      desktop:
        place: desktop::administration::settings-startup
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

# Start MedReminder at Windows login

## Trigger

An administrator wants reminders to arrive without opening MedReminder each time.

## Outcome

MedReminder starts hidden in the notification area at every Windows login of this account, with the last used profile.
