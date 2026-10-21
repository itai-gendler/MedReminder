---
appliesTo:
  - type: entity
    id: sync-group
    effect: removes
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

# Only an administrator disables sync

Disabling sync stops it on this PC only; the data stays as it is.
