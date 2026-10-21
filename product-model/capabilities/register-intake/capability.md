---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/RegisterIntake.cs#RegisterIntake
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/IntakeDialog.cs#IntakeDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Register intake

Record a single intake, taken, skipped or cancelled, for a day that differs from the schedule, or an extra dose taken as needed.
