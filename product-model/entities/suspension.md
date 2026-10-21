---
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/MedicationSuspension.cs#MedicationSuspension
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/SuspensionState.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Suspension

A temporary pause of a medicine's therapy, with an open or closed end. While a suspension covers a day nothing is deducted, no warning comes and no run-out date is estimated. On the desktop, suspensions arrive from the other devices of the profile's sync group.

## Information kept

- **From** — the first day of the pause
- **To** — the last day, empty while the pause is ongoing
- **Reason** — optional

## States

### Ongoing

The pause has no end date yet.

### Ended

The pause has an end date.
