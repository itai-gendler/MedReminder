---
entities:
  - entity: email-account
    shows:
      - Host
      - Port
      - Use StartTLS
      - Username
      - Sender
      - Timeout
    collects:
      - Host
      - Port
      - Use StartTLS
      - Username
      - Password
      - Sender
      - Timeout
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Email.cs
---

# Email SMTP settings

The Email SMTP section of Settings: the account every e-mail is sent from.
