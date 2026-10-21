---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Collected today on a prescription with one dispensation left
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Dispensations allowed
      - entity: dispensation
        effect: reads
        facts:
          - Collected on
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: The Product records the dispensation, and the prescription is collected
    kind: product
    actor: user
    entities:
      - entity: dispensation
        effect: creates
        facts:
          - Collected on
          - Packages
      - entity: prescription
        effect: changes
        from: To collect
        to: Collected
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#RecordDispensation
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DispensationEditDialog.cs#DispensationEditDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionsDialog.cs#PrescriptionsDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record the last dispensation

## Trigger

One dispensation is left.

## Outcome

Every dispensation is collected and the prescription is Collected.
