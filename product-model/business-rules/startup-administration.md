---
appliesTo:
  - type: entity
    id: preferences
    effect: changes
    facts:
      - Start at Windows login
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator changes the start at Windows login

The start at login belongs to the Windows account, so it would change the behaviour for every profile of that account.
