---
singleton: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Email/SmtpSettings.cs#SmtpSettings
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateSmtpSettings
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# E-mail account

The one SMTP account MedReminder sends every e-mail from. It is shared by every profile and, with a shared installation, by every device.

## Information kept

- **Host** — the outgoing server
- **Port** — 587 with StartTLS, or 465
- **Use StartTLS** — whether the connection is upgraded to encryption
- **Username** — the e-mail account
- **Password** — kept encrypted and never written to the logs
- **Sender** — the From address and sender name
- **Timeout** — seconds before giving up
