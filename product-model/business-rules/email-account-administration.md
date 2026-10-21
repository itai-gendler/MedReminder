---
appliesTo:
  - type: entity
    id: email-account
    effect: changes
permits:
  - actors:
      - administrator
  - unattended: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateSmtpSettings
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator changes the e-mail account

The Email SMTP section is shown to administrators only; with a shared installation the account changed on one device reaches the others.
