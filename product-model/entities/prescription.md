---
domain: prescriptions
relations:
  - entity: dispensation
    verb: has
    cardinality: one-to-many
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/Prescription.cs#Prescription
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/PrescriptionRules.cs#PrescriptionRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Prescription

A prescription the person follows from the request to the doctor to the pharmacy. Every date is optional, so it can be recorded at any stage. A repeatable prescription covers several dispensations over a long validity.

## Information kept

- **Requested on** — when the doctor was asked
- **Issued on** — when the doctor issued it
- **Prescription code** — as printed on the slip, for example the Italian NRE
- **Packages** — how many packages it covers, if known
- **Valid until** — the last day the pharmacy accepts it; filled in for 30 days from the issue date, or 12 months for a repeatable one, and changeable
- **Collected on** — when a single prescription was taken to the pharmacy
- **Repeatable** — whether it covers several dispensations
- **Dispensations allowed** — for a repeatable prescription, 2 to 12

## States

### Requested

Asked for and not issued yet.

### To collect

Issued and not collected yet (for a repeatable one: dispensations are left) while it is still valid.

### Collected

Taken to the pharmacy; for a repeatable one, every dispensation is collected.

### Expired

Its last valid day passed before it was collected.
