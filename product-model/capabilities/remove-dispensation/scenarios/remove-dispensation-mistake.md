---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the dispensation, click Remove and save
    kind: actor
    actor: user
    entities:
      - entity: dispensation
        effect: reads
        facts:
          - Collected on
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product removes the dispensation
    kind: product
    actor: user
    entities:
      - entity: dispensation
        effect: removes
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
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Remove a mistaken dispensation

## Trigger

A dispensation was recorded twice.

## Outcome

The dispensation is gone and one more is left to collect.
