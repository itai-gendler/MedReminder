---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: >-
      Enter host smtp.gmail.com, port 587 with Use StartTLS, the Gmail address as username and sender, and the
      app password as new password
    kind: actor
    actor: administrator
    entities:
      - entity: email-account
        effect: reads
        facts:
          - Host
          - Port
          - Use StartTLS
          - Username
          - Sender
    contexts:
      desktop:
        place: desktop::administration::settings-email
  - text: Click Save SMTP settings
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::administration::settings-email
  - text: The Product saves the account and keeps the password encrypted, never in the settings file
    kind: product
    actor: administrator
    entities:
      - entity: email-account
        effect: changes
        facts:
          - Host
          - Port
          - Use StartTLS
          - Username
          - Password
          - Sender
          - Timeout
    contexts:
      desktop:
        place: desktop::administration::settings-email
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateSmtpSettings
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Email.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Credentials/SmtpCredentialStore.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Set up a Gmail account with an app password

## Trigger

An administrator wants MedReminder to send e-mail.

## Outcome

The account is saved, its password encrypted; with a shared installation it reaches every device.

## Edge cases

- Outlook.com, Hotmail, Live and MSN accounts accept only modern sign-in, which MedReminder does not support: another account is used for sending.
