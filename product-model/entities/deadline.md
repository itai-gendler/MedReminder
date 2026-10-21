---
domain: administrative-deadlines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Deadlines/Deadline.cs#Deadline
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Deadlines/DeadlineRules.cs#DeadlineRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Deadline

An administrative date that is not about stock: the renewal of a therapeutic plan or an exemption, a periodic check-up, or anything else the person describes. It belongs to the profile and may concern one medicine.

## Information kept

- **Kind** — Therapeutic plan, Exemption renewal, Check-up or Other
- **Description** — optional, except for the kind Other
- **Medicine** — the medicine it concerns, or none for the whole profile
- **Date** — the date printed on the documents
- **Remind days before** — the notice period, 14 days by default
- **Repeat every** — months between two dates of a recurring deadline
- **Notify by** — Windows notification and/or Email
- **Done on** — when a one-off deadline was done

## States

### Upcoming

The notice period has not started.

### Due soon

The notice period has started and the date has not passed.

### Overdue

The date has passed and the deadline is not done.

### Done

A one-off deadline was marked done.
