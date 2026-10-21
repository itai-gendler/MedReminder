---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionReminders.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/RepeatablePrescriptionNotices.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/PrescriptionRules.cs#PrescriptionRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Remind to collect a prescription

Remind once, from 3 days before its last valid day, that an issued prescription has not been collected yet.
