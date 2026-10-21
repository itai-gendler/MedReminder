---
appliesTo:
  - type: entity
    id: medicine
    facts:
      - Administration times
  - type: capability
    id: register-intake
  - type: capability
    id: plan-supply
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/DailyConsumption.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# As-needed doses leave the stock only when registered

An as-needed slot is not part of the daily total; an as-needed (PRN) medicine tracks its stock without a run-out date, and its doses leave the stock when an intake is registered.
