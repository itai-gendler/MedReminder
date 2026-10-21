---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Select another profile that is synced and click Rename
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
  - text: The profile is not the open one and takes part in sync
    kind: condition
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Storage
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
  - text: The Product refuses and asks to open that profile to rename it
    kind: product
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts: []
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

# Rename a synced profile only while it is open

## Trigger

An administrator renames another profile that is synced with other devices.

## Outcome

Nothing changes; the administrator is told to open that profile first, so the new name reaches its devices too.
