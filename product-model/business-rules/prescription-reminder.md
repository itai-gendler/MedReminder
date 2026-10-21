---
appliesTo:
  - type: capability
    id: remind-prescription-to-collect
  - type: entity
    id: prescription
    facts:
      - Valid until
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionReminders.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A prescription to collect is reminded once, 3 days before it lapses

The reminder comes once per prescription and date, through the medicine's channels, and never includes the prescription code; a repeatable prescription is reminded only while dispensations are left.
