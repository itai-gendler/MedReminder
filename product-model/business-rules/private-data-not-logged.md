---
appliesTo:
  - type: entity
    id: email-account
    facts:
      - Password
  - type: entity
    id: medicine
    facts:
      - Notes
  - type: entity
    id: prescription
    facts:
      - Prescription code
references:
  - kind: doc
    role: intent
    target: README.md
  - kind: doc
    role: context
    target: CLAUDE.md
---

# Passwords, e-mail content and medical notes never reach the logs

The daily log records checks, e-mails sent, sync outcomes and errors, never passwords, message text, medicine notes or prescription codes.
