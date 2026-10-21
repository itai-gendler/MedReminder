---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select a user profile and click Change role…
    kind: actor
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
      - entity: user
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
  - text: The Product asks to confirm, recommending a PIN when the profile has none
    kind: product
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
  - text: Confirm
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
  - text: The Product makes the profile an administrator
    kind: product
    actor: administrator
    entities:
      - entity: user
        effect: removes
      - entity: administrator
        effect: creates
        facts: []
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#ChangeProfileRole
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Make a profile an administrator

## Trigger

A second family member should manage the installation too.

## Outcome

The profile is an administrator; with a shared installation the change reaches every device.
