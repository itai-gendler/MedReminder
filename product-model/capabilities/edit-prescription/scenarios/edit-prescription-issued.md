---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the requested prescription and click Edit…
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Requested on
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: Tick Issued on, enter the date, the code and how many it covers, check Valid until and save
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Issued on
          - Prescription code
          - Packages
          - Valid until
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product saves the prescription
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: changes
        from: Requested
        to: To collect
        facts:
          - Issued on
          - Prescription code
          - Packages
          - Valid until
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
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Complete a requested prescription once issued

## Trigger

A prescription recorded as requested has been issued.

## Outcome

The prescription is To collect, with its code and validity.

## Edge cases

- A prescription cannot move to another medicine.
