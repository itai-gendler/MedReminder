---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Select the profile, click Change PIN, tick Clear PIN and confirm
    kind: actor
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
  - text: The Product clears the profile's PIN
    kind: product
    actor: administrator
    entities:
      - entity: profile
        effect: changes
        facts:
          - PIN
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#SetProfilePin
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ChangePinDialog.cs#ChangePinDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Clear a forgotten PIN

## Trigger

A person forgot their PIN.

## Outcome

The profile opens without a PIN; with a shared installation the change reaches the other devices.
