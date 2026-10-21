---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose File → Change profile…
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Choose the profile and confirm
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
          - Last used
    contexts:
      desktop:
        place: desktop::profile::change-profile
  - text: The Product restarts and asks the PIN of the chosen profile
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::change-profile
  - text: Type the PIN
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::change-profile
  - text: The Product opens the main window on the chosen profile
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: changes
        facts:
          - Last used
    contexts:
      desktop:
        place: desktop::profile::medicines
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

# Open a parent's profile with its PIN

## Trigger

The person looks after a parent's medicines.

## Outcome

MedReminder restarts on the chosen profile after its PIN; at the next Windows startup that profile opens.
