---
entities:
  - entity: deadline
    shows:
      - Kind
      - Description
      - Medicine
      - Date
      - Remind days before
      - Repeat every
      - Notify by
      - Done on
    collects:
      - Kind
      - Description
      - Medicine
      - Date
      - Remind days before
      - Repeat every
      - Notify by
      - Done on
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DeadlineEditDialog.cs#DeadlineEditDialog
---

# Deadline window

One deadline being recorded or changed.
