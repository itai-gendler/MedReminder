---
appliesTo:
  - type: entity
    id: profile
    effect: changes
    facts:
      - Recipient
      - Caregiver e-mail
      - Copy to the caregiver
      - Weekly stock summary
      - Doctor e-mail
      - Package expiry notice
      - Region
      - Text size
      - Appearance
      - Last used
permits:
  - related:
      - verb: uses
        entity: user
  - related:
      - verb: uses
        entity: administrator
  - unattended: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs#UpdateNotificationSettings
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A person sets their own profile's recipients and appearance

The Notifications and General sections change the open profile only: its recipients, caregiver, doctor address, package expiry notice, region, text size and appearance. Sync brings the same settings from the profile's other devices, and the weekly summary records when it was sent.
