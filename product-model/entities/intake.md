---
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/MedicationIntake.cs#MedicationIntake
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/IntakeStatus.cs#IntakeStatus
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Intake

A single intake the person registers for a day that differs from the schedule. Once a day has an intake, what was registered replaces that day's automatic deduction, unless it is an extra dose.

## Information kept

- **Day** — the day the intake refers to
- **Quantity** — how much was taken, or would have been
- **Status** — Taken, Skipped or Cancelled
- **Extra dose** — a dose taken as needed on top of the schedule; the day's scheduled doses still count
- **Notes** — optional
