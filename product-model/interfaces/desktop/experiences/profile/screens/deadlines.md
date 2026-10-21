---
entities:
  - entity: deadline
    shows:
      - Kind
      - Description
      - Medicine
      - Date
      - Repeat every
      - Done on
  - entity: medicine
    shows:
      - Name
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DeadlinesDialog.cs#DeadlinesDialog
---

# Deadline list

The profile's administrative deadlines with their status, date, recurrence and when they were done; overdue ones stand out.
