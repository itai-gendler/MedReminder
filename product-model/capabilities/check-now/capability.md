---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/MedicationMonitor.cs#MedicationMonitor
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Tray/ApplicationTrayIcon.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/ConsumptionCatchUp.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Check now

Run the periodic check of the medicines at once, instead of waiting for the next one: catch up the daily deductions, then send whatever warning, reminder or notice is due.
