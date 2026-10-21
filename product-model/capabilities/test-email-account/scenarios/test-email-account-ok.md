---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Test connection
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
    contexts:
      desktop:
        place: desktop::administration::settings-email
  - text: The Product connects and signs in without sending, and reports the result
    kind: product
    actor: administrator
    entities:
      - entity: email-account
        effect: reads
        facts:
          - Host
          - Port
          - Use StartTLS
          - Username
    contexts:
      desktop:
        place: desktop::administration::settings-email
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

# Test the connection before saving

## Trigger

An administrator entered the account and wants to know it works.

## Outcome

MedReminder says whether it could connect and sign in; nothing is sent.

## Edge cases

- With Use StartTLS ticked, or with a username, a server that offers no encrypted connection is refused and the test says so.
