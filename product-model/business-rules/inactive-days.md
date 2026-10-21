---
appliesTo:
  - type: capability
    id: deactivate-medicine
  - type: capability
    id: reactivate-medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/MedicineActivity.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Inactive days are not consumed

The days a medicine is inactive are not counted as consumption, so reactivating it does not deduct the pause.
