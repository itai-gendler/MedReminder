---
entities:
  - entity: medicine
    shows:
      - Name
  - entity: prescription
    shows:
      - Requested on
      - Issued on
      - Valid until
      - Packages
      - Prescription code
      - Collected on
      - Dispensations allowed
  - entity: dispensation
    shows:
      - Collected on
  - entity: profile
    shows:
      - Region
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PrescriptionsDialog.cs#PrescriptionsDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/RegionalServicePanel.cs#RegionalServicePanel
---

# Prescription list

The prescriptions recorded for the profile's medicines, the ones still to collect first, with the regional prescription service for Italy.
