---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click My PIN, type the new PIN twice and confirm
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::settings-notifications
  - text: The Product saves the open profile's PIN
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: changes
        facts:
          - PIN
    contexts:
      desktop:
        place: desktop::profile::settings-notifications
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

# Set a PIN on one's own profile

## Trigger

The person wants a PIN asked when their profile opens.

## Outcome

The PIN is asked the next time the profile opens.
