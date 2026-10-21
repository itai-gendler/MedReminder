---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Open in mail client
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescription-request
  - text: The Product opens a new e-mail in the mail client with the subject and message
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Doctor e-mail
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

# Open the request in the mail client

## Trigger

No doctor e-mail or e-mail account is set up, this device is not the master device, or the person prefers their own mail program.

## Outcome

A new e-mail with the subject and message opens in the person's usual mail program.

## Edge cases

- A message too long for the mail client, or no mail client at all, is copied to the clipboard instead.
