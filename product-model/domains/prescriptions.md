---
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionsDialog.cs#PrescriptionsDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionRequestDialog.cs#PrescriptionRequestDialog
---

# Prescriptions

The Prescriptions and Prescription request windows: asking the doctor, then following each prescription and its dispensations to the pharmacy.

## Boundary

This domain does not own the stock a collected prescription brings in, which is recorded as a new package of the medicine.
