---
entities:
  - entity: medicine
    shows:
      - Name
      - Schedule
  - entity: intake
    collects:
      - Day
      - Quantity
      - Status
      - Extra dose
      - Notes
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/IntakeDialog.cs#IntakeDialog
---

# Intake window

One intake of the selected medicine, with the suggested dose.
