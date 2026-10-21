---
appliesTo:
  - type: capability
    id: delete-medicine
  - type: capability
    id: deactivate-medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/DeleteMedicine.cs#DeleteMedicine
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A medicine with history is deactivated, not deleted

Delete removes only a medicine with no stock movement, intake, count or suspension; otherwise the medicine is deactivated so its history is never lost by accident.
