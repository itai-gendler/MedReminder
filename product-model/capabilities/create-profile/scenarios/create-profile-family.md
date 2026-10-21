---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Choose Tools → Manage profiles…
    kind: actor
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Click New profile, type the name, keep the role User and optionally a PIN, then Create
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
  - text: The Product creates the profile with the user role
    kind: product
    actor: administrator
    entities:
      - entity: profile
        with: user
        effect: creates
        facts:
          - Name
          - PIN
      - entity: user
        effect: creates
        facts: []
    contexts:
      desktop:
        place: desktop::administration::manage-profiles
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

# Add a profile for a parent

## Trigger

An administrator also follows a parent's medicines.

## Outcome

The new profile, a user by default, has its own medicines and recipients and is opened with Change profile.

## Decision points

### Role of the new profile

Which role does the administrator choose?

- User → the profile sees only its own data
- Admin → the profile also manages the e-mail account, backup and the other profiles
