---
appliesTo:
  - type: entity
    id: preferences
    effect: changes
    facts:
      - Interface language
      - Check for app and catalogue updates
      - New medicine warning
      - Default dose times
permits:
  - actors:
      - user
      - administrator
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateGeneralSettings
  - kind: code
    role: implementation
    target: src/MedReminder.Application/DoseTimes/DoseTimeSettings.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Every profile sets the language, update check and dose times of this PC

Interface language, the update check, the warning new medicines start with and the dose times of medicines without slots belong to this PC, and any profile changes them.
