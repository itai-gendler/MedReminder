---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/DoseReminderService.cs#DoseReminderService
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/DoseReminderHostedService.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Remind at dose time

Show "Time to take …" at each timed slot of a medicine with Remind at dose time, at most once per slot per day. The reminder records nothing and does not change the stock.
