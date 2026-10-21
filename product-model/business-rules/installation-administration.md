---
appliesTo:
  - type: entity
    id: installation
    effect: changes
permits:
  - actors:
      - administrator
  - unattended: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdSync.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator changes the shared installation

Publishing, joining, handing over and changing keys happen in Tools → Installation…, shown to administrators only; each device also syncs the installation by itself.
