---
entities:
  - entity: profile
    collects:
      - Name
      - PIN
  - entity: installation
    collects:
      - Storage
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/FirstRunWizardForm.cs#FirstRunWizardForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Welcome to MedReminder

The first window on a PC where MedReminder was never used: create the first profile, which is an administrator, or join an installation already used on another PC.
