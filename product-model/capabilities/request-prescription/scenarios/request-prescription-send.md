---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine and open the request to the doctor
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: >-
      The Product prepares a message with the medicine name and pack, the product code and the person's name,
      greeting the reference doctor by name; no dosage or notes
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Package
          - Product code
          - Reference doctor
      - entity: profile
        effect: reads
        facts:
          - Name
          - Doctor e-mail
    contexts:
      desktop:
        place: desktop::profile::prescription-request
  - text: Edit the message if needed and click Send…
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Doctor e-mail
    contexts:
      desktop:
        place: desktop::profile::prescription-request
  - text: >-
      The e-mail account is set up, a doctor e-mail is filled in, and this device sends the installation's
      e-mail
    kind: condition
    entities:
      - entity: email-account
        effect: reads
        facts:
          - Host
      - entity: profile
        effect: reads
        facts:
          - Doctor e-mail
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescription-request
  - text: Confirm sending to the doctor's address
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescription-request
  - text: The Product sends the message and says it was sent
    kind: product
    actor: user
    entities:
      - entity: email-account
        effect: reads
        facts:
          - Host
          - Sender
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

# Send the request to the doctor

## Trigger

A medicine is running low and the person asks the doctor for a new prescription.

## Outcome

The request is sent from MedReminder's account to the doctor's address, after the person confirmed it.

## Edge cases

- Neither the subject, the message nor the recipient is written to the logs.
