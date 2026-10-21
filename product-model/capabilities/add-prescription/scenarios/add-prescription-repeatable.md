---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Tick Repeatable prescription and enter the dispensations allowed (2 to 12) and the issue date
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Repeatable
          - Dispensations allowed
          - Issued on
      - entity: dispensation
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product fills in Valid until for 12 months from the issue date
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
  - text: Save
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product records the repeatable prescription
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

# Record a repeatable prescription

## Trigger

The prescription covers several dispensations over a long validity, for example a year collected one month at a time.

## Outcome

One prescription is recorded with its dispensations allowed, valid for 12 months from the issue date unless changed.

## Edge cases

- On synced PCs every PC of the profile must be updated first: an older version stops syncing until it is updated.
- MedReminder does not check the interval between dispensations.
