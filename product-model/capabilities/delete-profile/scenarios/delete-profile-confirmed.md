---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the profile and click Delete
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
  - text: >-
      Type the profile name to confirm, optionally tick Also delete the profile's data on disk, and click
      Delete
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
  - text: The Product deletes the profile
    kind: product
    actor: administrator
    entities:
      - entity: profile
        with: user
        effect: removes
      - entity: user
        effect: removes
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#DeleteProfile
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Delete a profile no longer needed

## Trigger

A person no longer uses MedReminder on this installation.

## Outcome

The profile is gone from the lists; its database file stays on disk for manual recovery unless deleting it was ticked.
