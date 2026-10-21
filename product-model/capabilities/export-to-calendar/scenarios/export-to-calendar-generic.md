---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Therapy → Export to calendar…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Leave Include medicine names and deadline descriptions off and save the file
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
  - text: >-
      The Product writes, for each active medicine, the day to request the prescription (the run-out date less
      the warning threshold) and the run-out date, the last day to collect each prescription and the open
      deadlines
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Run-out date
          - Warning threshold
      - entity: prescription
        effect: reads
        facts:
          - Valid until
      - entity: deadline
        effect: reads
        facts:
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

# Export the coming dates without names

## Trigger

The person wants the dates in their calendar.

## Outcome

An .ics file holds reminder events that only say what to do; exporting again later updates the same events.

## Edge cases

- The events are reminders, not appointments: they do not make the person busy.
