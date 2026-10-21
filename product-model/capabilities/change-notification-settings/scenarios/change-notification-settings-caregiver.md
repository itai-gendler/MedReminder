---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Tools → Settings… and the Notifications section
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: >-
      Enter the recipient and the caregiver e-mail, tick the kinds of e-mail to copy and, if wanted, the
      weekly stock summary
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Recipient
          - Caregiver e-mail
          - Copy to the caregiver
          - Weekly stock summary
    contexts:
      desktop:
        place: desktop::profile::settings-notifications
  - text: Click Save recipients
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::settings-notifications
  - text: >-
      The Product saves the recipients of the open profile; with sync on, they reach the profile's other
      devices
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: changes
        facts:
          - Recipient
          - Caregiver e-mail
          - Copy to the caregiver
          - Weekly stock summary
      - entity: device
        effect: reads
        facts: []
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

# Add a caregiver who gets copies

## Trigger

A family member wants to know when the person's medicines run low.

## Outcome

The caregiver receives a copy of the chosen kinds of e-mail, in the same e-mail as the recipient, and optionally a weekly stock summary.

## Edge cases

- Both addresses are visible to both recipients.
- The doctor e-mail is used only for prescription requests sent by the person; automatic reminders never go there.
