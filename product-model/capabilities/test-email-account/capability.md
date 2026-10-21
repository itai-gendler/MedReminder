---
availability:
  - place: desktop::administration
domain: email
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Email/SmtpConnectionTester.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Email/SmtpSecurity.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Email.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Test the e-mail connection

Check that MedReminder can sign in to the SMTP server, without sending anything.
