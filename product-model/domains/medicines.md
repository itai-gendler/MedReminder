---
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
---

# Medicines

The Medicines page: the list of the profile's medicines and what the person does on one of them, from its therapy to its stock, intakes, counts and history.

## Boundary

This domain does not own the boxes in the cabinet (Packages), prescriptions, administrative deadlines or the reminders sent from the periodic check.
