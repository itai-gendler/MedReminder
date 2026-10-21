---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Tick Include medicine names and deadline descriptions and save the file
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
      - entity: deadline
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::export-to-calendar
  - text: The Product writes the same events with the names
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Run-out date
      - entity: deadline
        effect: reads
        facts:
          - Description
          - Date
    contexts:
      desktop:
        place: desktop::profile::export-to-calendar
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Calendar/CalendarExportQuery.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Calendar/IcsWriter.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/CalendarExportDialog.cs#CalendarExportDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Export the dates with the names

## Trigger

The person accepts that the calendar shows the medicine names.

## Outcome

The events carry the medicine names and deadline descriptions; the choice is asked at every export.
