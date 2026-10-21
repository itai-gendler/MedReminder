---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Paste NRE next to Prescription code
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: >-
      The Product reads the clipboard only now, writes the NRE without spaces and fills an empty issue date
      with today
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

# Paste the NRE copied from the regional service

## Trigger

The person copied the electronic prescription number from their region's service.

## Outcome

The code is the 15-character NRE without spaces, and an empty Issued on becomes today.

## Edge cases

- A clipboard that holds no NRE (15 letters or digits) is refused.
