---
appliesTo:
  - type: capability
    id: change-profile
  - type: entity
    id: profile
    facts:
      - PIN
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PinPromptForm.cs#PinPromptForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# One profile is open at a time

Changing profile restarts MedReminder on the chosen one, after its PIN if it has one; three wrong PINs close MedReminder.
