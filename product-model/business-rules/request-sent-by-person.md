---
appliesTo:
  - type: capability
    id: request-prescription
  - type: entity
    id: profile
    facts:
      - Doctor e-mail
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/SendPrescriptionRequest.cs#SendPrescriptionRequest
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionRequestTexts.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A prescription request leaves only when the person sends it

MedReminder never sends a request by itself and never contacts the doctor; the request carries the medicine, package, product code and the person's name, never the dosage or the notes. Automatic reminders never go to the doctor's address.
