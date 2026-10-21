---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the prescription, click Delete and confirm
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
  - text: The Product removes the prescription
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: removes
        from: Requested
    contexts:
      desktop:
        place: desktop::profile::prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#DeletePrescription
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionsDialog.cs#PrescriptionsDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Delete a prescription recorded twice

## Trigger

A prescription was recorded twice or for the wrong medicine.

## Outcome

The prescription is gone from the list and gives no reminders.
