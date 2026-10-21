---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/DeactivateMedicine.cs#DeactivateMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/MedicineActivity.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Deactivate a medicine

Stop a therapy: the medicine is hidden and gets no more reminders, and its history is kept.
