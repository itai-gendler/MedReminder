---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Add… under Dispensations collected, enter the day and how many were dispensed, and save
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
  - text: The Product records the dispensation
    kind: product
    actor: user
    entities:
      - entity: dispensation
        effect: creates
        facts:
          - Collected on
          - Packages
    contexts:
      desktop:
        place: desktop::profile::prescription
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

# Add a dispensation with its packages

## Trigger

The person records a past dispensation with the number of packages.

## Outcome

The dispensation is added with its day and packages.

## Edge cases

- A dispensation cannot be before the issue date or after Valid until, and no more can be recorded than the prescription allows.
