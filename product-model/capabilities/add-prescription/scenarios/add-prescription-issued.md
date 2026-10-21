---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click New… in the list of prescriptions
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: Choose the medicine, tick Issued on and enter the date, the prescription code and how many it covers
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Issued on
          - Prescription code
          - Packages
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product fills in Valid until for 30 days from the issue date
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Valid until
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: Check Valid until against the slip and save
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product records the prescription
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: creates
        to: To collect
        facts:
          - Requested on
          - Issued on
          - Prescription code
          - Packages
          - Valid until
          - Collected on
          - Repeatable
          - Dispensations allowed
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

# Record an issued prescription

## Trigger

The doctor issued a prescription.

## Outcome

The prescription is To collect, valid for 30 days from the issue date unless the person changed it.

## Edge cases

- At least one date is needed: requested, issued or collected.
- The issue date cannot be before the request date, nor Valid until or the collection before the issue date.
