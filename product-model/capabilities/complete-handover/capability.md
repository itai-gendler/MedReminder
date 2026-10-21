---
availability:
  - place: desktop::administration
domain: installation
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/MasterHandover.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HandoverWizardForm.cs#HandoverWizardForm
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

# Complete the handover

On the elected master, check what it needs and confirm: a working e-mail connection from this device, the cloud backup sign-in and passphrase, and the profiles to bring.
