---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the prescription to collect and click Collected today
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Issued on
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
        from: To collect
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

# Collect a prescription today

## Trigger

The person took the prescription to the pharmacy.

## Outcome

The prescription is Collected today and gives no more reminders.
