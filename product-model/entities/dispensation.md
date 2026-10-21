---
domain: prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/PrescriptionDispensation.cs#PrescriptionDispensation
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Dispensation

One collection at the pharmacy under a repeatable prescription.

## Information kept

- **Collected on** — the day of the collection
- **Packages** — how many packages were dispensed, if known
