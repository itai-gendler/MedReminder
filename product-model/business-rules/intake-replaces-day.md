---
appliesTo:
  - type: capability
    id: register-intake
  - type: entity
    id: intake
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/RegisterIntake.cs#RegisterIntake
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A registered intake replaces that day's deduction

Once an intake, taken, skipped or cancelled, is registered for a day, that day's automatic deduction is replaced by what was registered. An extra dose as needed is deducted on top and leaves the day's scheduled doses in place.
