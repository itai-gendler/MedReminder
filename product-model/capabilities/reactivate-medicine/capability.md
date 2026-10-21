---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/UpdateMedicine.cs#UpdateMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/MedicineActivity.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Reactivate a medicine

Resume a stopped therapy. The days the medicine was inactive are not counted as consumption.
