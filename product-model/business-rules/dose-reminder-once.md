---
appliesTo:
  - type: capability
    id: remind-dose-time
  - type: capability
    id: snooze-dose-reminder
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/DoseReminderService.cs#DoseReminderService
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A dose reminder comes at most once per slot per day

Each timed slot is reminded once a day, even across restarts, within 30 minutes of its time; a slot missed for longer is skipped. The reminder records nothing.
