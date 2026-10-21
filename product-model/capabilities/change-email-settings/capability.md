---
availability:
  - place: desktop::administration
domain: email
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

# Change Email SMTP settings

Set the SMTP account every e-mail is sent from: host, port, StartTLS, username, password, sender and timeout.
