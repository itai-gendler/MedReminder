---
entities:
  - entity: medicine
    shows:
      - Name
      - Run-out date
      - Warning threshold
  - entity: prescription
    shows:
      - Valid until
  - entity: deadline
    shows:
      - Date
      - Description
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/CalendarExportDialog.cs#CalendarExportDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Calendar/CalendarExportQuery.cs
---

# Calendar export window

Saves the coming dates as a calendar file, with or without the medicine names.
