---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Select the last administrator, or the open profile, and click Change role…
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
  - text: It is the open profile, or no other administrator would remain
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
  - text: The Product refuses and says why
    kind: product
    actor: administrator
    entities: []
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

# Keep the last administrator

## Trigger

An administrator tries to make the only other administrator a user, or to change the open profile's role.

## Outcome

Nothing changes: at least one administrator must remain, and the open profile's role changes only from another administrator profile.
