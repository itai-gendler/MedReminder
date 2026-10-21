---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Choose the reference country and save the section
    kind: actor
    actor: administrator
    entities:
      - entity: preferences
        effect: reads
        facts:
          - Reference country
    contexts:
      desktop:
        place: desktop::profile::settings-general
  - text: The Product saves the country for the whole installation
    kind: product
    actor: administrator
    entities:
      - entity: preferences
        effect: changes
        facts:
          - Reference country
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

# Change the reference country

## Trigger

An administrator's family lives in another country.

## Outcome

Every profile, and with a shared installation every device, uses the new country's catalogue.

## Edge cases

- For a user the reference country and database-query logging are shown but cannot be changed.
