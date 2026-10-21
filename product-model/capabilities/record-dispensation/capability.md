---
availability:
  - place: desktop::profile
domain: prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#RecordDispensation
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DispensationEditDialog.cs#DispensationEditDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionsDialog.cs#PrescriptionsDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record a dispensation

Record one collection under a repeatable prescription, from its editor, from the list with Collected today, or when a new package of its medicine is added.
