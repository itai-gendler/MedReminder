---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Therapy → Therapy report…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product prepares the card of the active medicines, without notes
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Active ingredient
          - Schedule
          - Administration times
          - Therapy start date
          - Therapy end date
          - Reference doctor
      - entity: profile
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::therapy-report
  - text: Choose A4 or Letter and click Print…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::therapy-report
  - text: The Product shows the print preview and prints
    kind: product
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::therapy-report
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Reporting/TherapyReport.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/TherapyReportDialog.cs#TherapyReportDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Print the therapy for a doctor

## Trigger

The person goes to a doctor or a pharmacy.

## Outcome

A card with active ingredient, dosage, therapy period and doctor of each active medicine is printed; MedReminder keeps no copy.

## Edge cases

- Include notes is off by default, since notes may be private.
