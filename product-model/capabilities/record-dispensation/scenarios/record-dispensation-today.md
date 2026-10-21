---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the repeatable prescription and click Collected today
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts:
          - Dispensations allowed
          - Valid until
      - entity: dispensation
        effect: reads
        facts:
          - Collected on
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: The Product records a dispensation today, leaving the rest of the prescription as it is
    kind: product
    actor: user
    entities:
      - entity: dispensation
        effect: creates
        facts:
          - Collected on
          - Packages
      - entity: prescription
        effect: reads
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

# Record today's dispensation from the list

## Trigger

The person collected one month of a repeatable prescription.

## Outcome

The list shows one more dispensation collected, such as 4 / 12; the last one makes the prescription Collected.

## Edge cases

- A repeatable prescription takes a dispensation only within its validity.
- Two devices recording a dispensation at the same time both keep theirs.
