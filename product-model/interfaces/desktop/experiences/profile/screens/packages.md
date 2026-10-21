---
entities:
  - entity: medicine
    shows:
      - Name
  - entity: package
    shows:
      - Status
      - Expiry
      - Opened on
      - Use within
      - Use until
      - In stock
      - Batch
      - Quantity
    collects:
      - Quantity
      - Expiry
      - Use within
      - Opened on
      - Batch
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackageEditDialog.cs#PackageEditDialog
---

# Packages of a medicine

The boxes of one medicine with their status, printed expiry, opening date, use-until date and quantity in stock; used-up and closed boxes on request.
