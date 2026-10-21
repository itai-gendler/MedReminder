---
availability:
  - place: desktop::profile
domain: prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#SavePrescription
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionEditDialog.cs#PrescriptionEditDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Edit a prescription

Complete a prescription as it moves on: the issue date, code, packages and validity once issued, or the collection date.
