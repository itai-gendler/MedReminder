---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Tools → Settings… and the General section
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Choose the interface language and save the section
    kind: actor
    actor: user
    entities:
      - entity: preferences
        effect: reads
        facts:
          - Interface language
    contexts:
      desktop:
        place: desktop::profile::settings-general
  - text: The Product saves the language for this PC and restarts
    kind: product
    actor: user
    entities:
      - entity: preferences
        effect: changes
        facts:
          - Interface language
    contexts:
      desktop:
        place: desktop::profile::settings-general
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateGeneralSettings
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.General.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Settings/ProfileUiSettingsFile.cs#ProfileUiSettingsFile
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change the interface language

## Trigger

The person prefers another of the five languages.

## Outcome

MedReminder restarts in the chosen language; e-mails, the therapy report and the user guide use it too.
