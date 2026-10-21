---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Change the days of the expiry notice and save
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Package expiry notice
    contexts:
      desktop:
        place: desktop::profile::settings-notifications
  - text: The Product saves the notice days of the profile
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: changes
        facts:
          - Package expiry notice
    contexts:
      desktop:
        place: desktop::profile::settings-notifications
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs#UpdateNotificationSettings
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs#UpdateProfileRegion
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Notifications.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Email/NotificationRecipientRules.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change when package expiry notices come

## Trigger

The person wants an earlier notice before boxes expire.

## Outcome

The "expires soon" notice comes the chosen number of days before the printed expiry (0 to 180) and before the end of the period after opening (0 to 30).
