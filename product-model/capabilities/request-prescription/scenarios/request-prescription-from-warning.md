---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Prepare request on the low-stock notification
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product opens the request for the medicine of the warning
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Package
          - Reference doctor
    contexts:
      desktop:
        place: desktop::profile::prescription-request
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionRequestDialog.cs#PrescriptionRequestDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionRequestTexts.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/SendPrescriptionRequest.cs#SendPrescriptionRequest
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/MailtoLink.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Prepare the request from a low-stock warning

## Trigger

A Windows low-stock warning appears and the person clicks Prepare request.

## Outcome

MedReminder opens on the request for that medicine.

## Edge cases

- For a medicine with a repeatable prescription that still has dispensations, the button opens the prescription instead.
