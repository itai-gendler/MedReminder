---
actors:
  - user
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#CollectPrescription
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record a collection with the new package

## Goal

Coming back from the pharmacy, the person records both the new stock and the prescription it came from.

## Success criterion

The stock is refilled and the prescription is marked collected, or one more dispensation is recorded.
