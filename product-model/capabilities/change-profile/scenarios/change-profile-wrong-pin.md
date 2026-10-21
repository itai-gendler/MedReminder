---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Type a wrong PIN three times
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::change-profile
  - text: No attempt is left
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile::change-profile
  - text: The Product says there were too many wrong attempts and closes
    kind: product
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::change-profile
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

# Close after three wrong PINs

## Trigger

The PIN typed is wrong three times.

## Outcome

MedReminder closes without opening the profile.
