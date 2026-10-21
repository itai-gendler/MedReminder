---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Select the expired single prescription and click Collected today
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Valid until
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: The Product records today as the collection date
    kind: product
    actor: user
    entities:
      - entity: prescription
        effect: changes
        from: Expired
        to: Collected
        facts:
          - Collected on
    contexts:
      desktop:
        place: desktop::profile::prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#CollectPrescription
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionsDialog.cs#PrescriptionsDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record a late collection

## Trigger

The person forgot to record a collection and the prescription now shows Expired.

## Outcome

A single prescription can still be marked collected after its last valid day.
