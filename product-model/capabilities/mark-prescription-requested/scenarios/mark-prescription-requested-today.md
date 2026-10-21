---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Mark as requested
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescription-request
  - text: The Product records a prescription of the medicine requested today
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: creates
        to: Requested
        facts:
          - Requested on
          - Issued on
          - Prescription code
          - Packages
          - Valid until
          - Collected on
          - Repeatable
          - Dispensations allowed
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescription-request
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionRequestDialog.cs#PrescriptionRequestDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#SavePrescription
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record the request with today's date

## Trigger

The person has sent or copied the request.

## Outcome

A prescription of the medicine is recorded as requested today and appears in Prescriptions.
