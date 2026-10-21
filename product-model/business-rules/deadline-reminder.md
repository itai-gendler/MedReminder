---
appliesTo:
  - type: capability
    id: remind-deadline
  - type: entity
    id: deadline
    facts:
      - Remind days before
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Deadlines/DeadlineReminders.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A deadline is reminded once per date from its notice period

From Remind days before the date on, one reminder comes per date through the deadline's own channels; MedReminder has no rule of its own for these dates.
