---
appliesTo:
  - type: entity
    id: sync-group
    effect: creates
permits:
  - actors:
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator enables or joins sync

Tools → Sync… is shown to administrators only; sync is set up for each profile with the profile open.
