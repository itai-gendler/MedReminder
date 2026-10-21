---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Create profile, type the name and, if wanted, a PIN twice
    kind: actor
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::welcome
  - text: The Product creates the profile as an administrator
    kind: product
    actor: administrator
    entities:
      - entity: profile
        with: administrator
        effect: creates
        facts:
          - Name
          - PIN
      - entity: administrator
        effect: creates
        facts: []
    contexts:
      desktop:
        place: desktop::profile::welcome
  - text: The Product opens the main window on the new profile
    kind: product
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#CreateProfile
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/FirstRunWizardForm.cs#FirstRunWizardForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Profiles/ProfileRegistry.cs#ProfileRegistry
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Create the first profile

## Trigger

MedReminder starts on a PC where it was never used.

## Outcome

The first profile exists as an administrator and the main window opens; while it has no medicine, the guided setup opens by itself.
