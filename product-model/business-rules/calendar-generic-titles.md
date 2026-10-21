---
appliesTo:
  - type: capability
    id: export-to-calendar
  - type: capability
    id: warn-low-stock
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Calendar/CalendarEntries.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Calendar events name no medicine unless asked

Calendars are often stored online by another company, so the exported events and the calendar file of a low-stock e-mail only say what to do, unless the person asks for the names at the export.
