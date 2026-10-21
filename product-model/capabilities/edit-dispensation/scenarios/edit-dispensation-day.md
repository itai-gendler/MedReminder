---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the dispensation, click Edit…, correct the day and save
    kind: actor
    actor: user
    entities:
      - entity: dispensation
        effect: reads
        facts:
          - Collected on
          - Packages
    contexts:
      desktop:
        place: desktop::profile::prescription
  - text: The Product saves the dispensation
    kind: product
    actor: user
    entities:
      - entity: dispensation
        effect: changes
        facts:
          - Collected on
          - Packages
    contexts:
      desktop:
        place: desktop::profile::prescription
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/PrescriptionUseCases.cs#SavePrescription
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DispensationEditDialog.cs#DispensationEditDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Correct the day of a dispensation

## Trigger

A dispensation was recorded on the wrong day.

## Outcome

The dispensation has the corrected day; a dispensation removed meanwhile on another device is not brought back.
