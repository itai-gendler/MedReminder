---
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
---

# Packages

The Packages window of a medicine: the boxes in the cabinet, their expiry, opening and closing.

## Boundary

This domain does not own the stock quantity, which changes only through stock movements, intakes, counts and the daily deduction.
