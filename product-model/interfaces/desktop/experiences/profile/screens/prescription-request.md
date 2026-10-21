---
entities:
  - entity: medicine
    shows:
      - Name
      - Package
      - Product code
      - Reference doctor
  - entity: profile
    shows:
      - Name
      - Doctor e-mail
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionRequestDialog.cs#PrescriptionRequestDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionRequestTexts.cs
---

# Prescription request

A short message to the doctor asking for a new prescription of the selected medicine, ready to edit, copy, open in the mail client or send.
