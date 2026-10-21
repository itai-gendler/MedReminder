---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Open the Email SMTP section on a PC that is not the master
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::administration::settings-email
  - text: This device is not the master device of the installation
    kind: condition
    entities:
      - entity: installation
        effect: reads
        facts:
          - Master device
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::settings-email
  - text: The Product says the e-mail is sent by the master device and to test the connection there
    kind: product
    actor: administrator
    entities:
      - entity: device
        effect: reads
        facts: []
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

# Test only on the master device

## Trigger

An administrator opens the Email SMTP section on a device of a shared installation that is not the master.

## Outcome

The settings can be edited and reach every device, but the test runs only on the master device, which sends the e-mail.
