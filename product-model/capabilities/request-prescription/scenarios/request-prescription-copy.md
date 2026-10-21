---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Copy
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescription-request
  - text: The Product copies the subject and message to the clipboard
    kind: product
    actor: user
    entities: []
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

# Copy the request for a portal or messaging app

## Trigger

The doctor takes requests through a patient portal, webmail or a messaging app.

## Outcome

The subject and message are on the clipboard, ready to paste.
