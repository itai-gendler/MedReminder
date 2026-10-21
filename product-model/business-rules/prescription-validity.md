---
appliesTo:
  - type: capability
    id: add-prescription
  - type: capability
    id: edit-prescription
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/PrescriptionRules.cs#PrescriptionRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Valid until starts at 30 days from the issue date, or 12 months for a repeatable prescription

The validity is only pre-filled, as validity differs between countries and kinds of prescription; the person checks it against the slip and changes it if needed. A repeatable prescription allows 2 to 12 dispensations.
