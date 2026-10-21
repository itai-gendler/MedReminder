---
appliesTo:
  - type: capability
    id: add-prescription
  - type: capability
    id: edit-prescription
  - type: capability
    id: record-dispensation
  - type: capability
    id: edit-dispensation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/PrescriptionRules.cs#PrescriptionRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Prescription dates follow request, issue and collection

A prescription has at least one date; it is not issued before it was requested, nor valid, collected or dispensed before it was issued; no dispensation comes after Valid until, and no more are recorded than allowed.
