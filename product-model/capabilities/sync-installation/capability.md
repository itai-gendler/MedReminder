---
availability:
  - place: desktop::profile
domain: installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/HouseholdHostedService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdProjection.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Household/MasterRules.cs
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

# Sync the installation

Every 15 minutes, bring to this device the changes of profiles, roles, PINs, the e-mail account, the cloud backup policy and the reference country made on the other devices.
