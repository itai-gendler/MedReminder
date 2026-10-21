---
availability:
  - place: desktop::administration
domain: installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdKeyring.cs
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

# Enter the new installation key

Give a device the installation's new key, with the new passphrase or a code, after a device was removed elsewhere.
