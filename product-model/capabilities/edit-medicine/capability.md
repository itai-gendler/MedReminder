---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/EditMedicine.cs#EditMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/UpdateMedicine.cs#UpdateMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/TherapyStartChange.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Edit a medicine

Change a medicine's name, package, warning threshold, channels, dose-time reminder, administration times, end date, doctor and notes.
