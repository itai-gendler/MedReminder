---
availability:
  - place: desktop::administration
domain: installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdPairingOffers.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/Remote/HouseholdPairing.cs
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

# Add a device

Choose the profiles a new device will hold and show the short-lived code it joins with.
