---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Save with no date, or with Valid until before the issue date
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Requested on
          - Issued on
          - Valid until
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The dates are missing or inconsistent
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product names the date to fix and records nothing
    kind: product
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescription
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#SavePrescription
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionEditDialog.cs#PrescriptionEditDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/PrescriptionRules.cs#PrescriptionRules
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/PrescriptionBarcodeReader.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/NreCode.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Refuse a prescription without dates

## Trigger

The person saves with no date ticked, or with dates in the wrong order.

## Outcome

Nothing is recorded and the window says which date to fix.
