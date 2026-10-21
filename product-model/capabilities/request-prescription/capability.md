---
availability:
  - place: desktop::profile
domain: prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionRequestDialog.cs#PrescriptionRequestDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionRequestTexts.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/SendPrescriptionRequest.cs#SendPrescriptionRequest
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/MailtoLink.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Request a prescription

Prepare a short message asking the doctor for a new prescription of a medicine, edit it, and copy it, open it in the mail client or send it with MedReminder's e-mail account. MedReminder never sends a request by itself.
