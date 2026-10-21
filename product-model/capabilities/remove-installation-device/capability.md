---
availability:
  - place: desktop::administration
domain: installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/RemoveDevice.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdSync.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HouseholdDialog.cs#HouseholdDialog
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Remove a device from the installation

Remove a lost, sold or given-away device on the master: the installation and the profiles it held move to new keys.
