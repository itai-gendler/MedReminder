---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Choose the text size and the appearance and save the section
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Text size
          - Appearance
    contexts:
      desktop:
        place: desktop::profile::settings-general
  - text: The Product saves them for the open profile on this PC
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: changes
        facts:
          - Text size
          - Appearance
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

# Make the text larger for this profile

## Trigger

The person reads better with larger text.

## Outcome

This profile shows text Large or Extra large; other profiles keep theirs.

## Edge cases

- Same as Windows is dark only on Windows 11 with dark mode on; a Windows high-contrast theme's colours are used; the appearance applies after a restart.
