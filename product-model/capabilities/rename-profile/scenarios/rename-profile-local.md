---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the profile, click Rename, type the new name and confirm
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
  - text: The Product renames the profile
    kind: product
    actor: administrator
    entities:
      - entity: profile
        effect: changes
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs#SyncedProfileRenameException
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Correct a profile's name

## Trigger

A profile has the wrong name.

## Outcome

The profile is shown with the new name.
