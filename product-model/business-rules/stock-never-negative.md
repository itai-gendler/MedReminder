---
appliesTo:
  - type: capability
    id: register-intake
  - type: capability
    id: correct-stock
  - type: capability
    id: discard-package
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/MedicineStock.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# The stock never goes below zero

An intake, a negative correction or a discard that would take more than the stock is refused.
