---
availability:
  - place: desktop::profile
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

# Export to calendar

Save the coming dates in a calendar file for Outlook, Google Calendar or a phone: when to request each prescription, when each medicine runs out, the last day to collect a prescription and the open administrative deadlines.
