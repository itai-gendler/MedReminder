---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Enter a caregiver e-mail equal to the recipient and save
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Recipient
          - Caregiver e-mail
    contexts:
      desktop:
        place: desktop::profile::settings-notifications
  - text: The Product asks for a different caregiver address
    kind: product
    actor: user
    entities: []
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

# Refuse a caregiver address equal to the recipient

## Trigger

The person enters the same address twice.

## Outcome

Nothing is saved until the caregiver address differs from the recipient.
