---
entities:
  - entity: medicine
    shows:
      - Name
  - entity: package
    shows:
      - Status
      - Use until
      - In stock
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ExpiringPackagesDialog.cs#ExpiringPackagesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/ExpiringPackagesQuery.cs#ExpiringPackagesQuery
---

# Expiring packages

The expired and expiring boxes of every medicine of the profile, including medicines no longer in use, expired first.
