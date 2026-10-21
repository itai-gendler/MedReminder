---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Scan next to Prescription code and scan the slip
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product reads the identifier the reference country uses and fills an empty issue date with today
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Prescription code
          - Issued on
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: Save
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

# Scan the code from the prescription slip

## Trigger

The person has the paper slip and a scanner or a webcam.

## Outcome

Only the prescription identifier is kept, and an empty Issued on becomes today.

## Decision points

### What is read

Which reference country is chosen?

- Italy → the NRE, printed as one barcode or as two scanned in any order, or the 12-character NRBE of a prescription outside the national health service; any other barcode on the slip, such as the tax code, is refused
- Germany → the 22-character prescription ID from the DataMatrix or QR code; the access code is never saved, and a code grouping several prescriptions is refused
- France → the prescription identifier from the QR code; the patient's name and birth date and the prescriber's numbers are never saved

## Edge cases

- With any other reference country the Scan button is not shown.
