---
appliesTo:
  - type: entity
    id: suspension
  - type: capability
    id: warn-low-stock
  - type: capability
    id: remind-dose-time
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/MedicationSuspension.cs#MedicationSuspension
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/RunOutForecast.cs
  - kind: doc
    role: intent
    target: README.md
---

# A suspended day is neither consumed nor warned

While a suspension covers a day, nothing is deducted, no low-stock warning or dose reminder comes, and no run-out date is estimated.
