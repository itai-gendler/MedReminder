---
entities:
  - entity: medicine
    shows:
      - Name
  - entity: prescription
    shows:
      - Requested on
      - Issued on
      - Prescription code
      - Packages
      - Valid until
      - Collected on
      - Repeatable
      - Dispensations allowed
    collects:
      - Requested on
      - Issued on
      - Prescription code
      - Packages
      - Valid until
      - Collected on
      - Repeatable
      - Dispensations allowed
  - entity: dispensation
    shows:
      - Collected on
      - Packages
    collects:
      - Collected on
      - Packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionEditDialog.cs#PrescriptionEditDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/DispensationEditDialog.cs#DispensationEditDialog
---

# Prescription window

One prescription being recorded or changed: its medicine, the dates known, the code (typed, pasted or scanned), the packages and, for a repeatable one, its dispensations.
