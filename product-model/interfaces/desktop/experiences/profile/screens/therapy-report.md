---
entities:
  - entity: medicine
    shows:
      - Name
      - Active ingredient
      - Schedule
      - Administration times
      - Therapy start date
      - Therapy end date
      - Reference doctor
      - Notes
  - entity: profile
    shows:
      - Name
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/TherapyReportDialog.cs#TherapyReportDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Reporting/TherapyReport.cs
---

# Therapy report

A card of the active medicines for a doctor, an emergency room or a pharmacist: active ingredient, dosage, therapy period and doctor, with the notes only on request.
