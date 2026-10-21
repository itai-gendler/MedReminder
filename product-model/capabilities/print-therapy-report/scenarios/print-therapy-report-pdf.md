---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Save as PDF…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::therapy-report
  - text: The Microsoft Print to PDF printer is installed
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile::therapy-report
  - text: The Product saves the PDF where the person chooses
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Schedule
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

# Save the therapy report as PDF

## Trigger

The person wants to send the report by e-mail.

## Outcome

The report is saved as a PDF through the Microsoft Print to PDF printer.

## Edge cases

- Without that printer the window says how to add it back; Save to file… and Copy to clipboard give plain text.
