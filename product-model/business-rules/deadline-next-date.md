---
appliesTo:
  - type: capability
    id: complete-deadline
  - type: entity
    id: deadline
    facts:
      - Date
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Deadlines/DeadlineRules.cs#DeadlineRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A recurring deadline moves on from its previous date

Done moves a recurring deadline forward by its recurrence, counted from the date it was due, not from the day it was marked, so a late renewal does not shift the whole series.
