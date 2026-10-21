---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilePickerForm.cs#ProfilePickerForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PinPromptForm.cs#PinPromptForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Program.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change profile

Open another person's profile. MedReminder restarts with it, so each profile is fully separate, and asks its PIN if it has one.
