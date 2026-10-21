---
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Sync/SyncConflict.cs#SyncConflict
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncConflictActions.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Sync conflict

A case where two devices changed the same thing before syncing: the latest change wins and the other value is kept here, so it can be brought back.

## Information kept

- **Detected** — when it was found
- **Medicine** — the medicine concerned
- **Case** — such as the same field edited on two devices, two schedule changes for the same date, overlapping suspensions, or intakes of one day above the dose
- **Field** — the field concerned
- **Kept value** — the value that won
- **Lost value** — the value that can be restored
