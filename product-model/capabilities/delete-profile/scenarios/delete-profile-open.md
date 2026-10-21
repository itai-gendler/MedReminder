---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Select the open profile or the last administrator
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
  - text: It is the open profile, or the last administrator
    kind: condition
    entities:
      - entity: administrator
        effect: reads
        facts: []
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
  - text: The Product keeps Delete unavailable and says why
    kind: product
    actor: administrator
    entities: []
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

# Keep the open profile and the last administrator

## Trigger

An administrator selects the open profile or the last administrator.

## Outcome

Delete is unavailable; another profile is opened first with Change profile.
