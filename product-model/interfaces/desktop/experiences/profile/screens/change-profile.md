---
entities:
  - entity: profile
    shows:
      - Name
      - Last used
    collects:
      - PIN
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilePickerForm.cs#ProfilePickerForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PinPromptForm.cs#PinPromptForm
---

# Choose profile

The profiles on this PC with their role and when each was last used; the PIN of the chosen profile is asked before it opens.
