---
singleton: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Abstractions/UserSettings.cs#UserSettings
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateGeneralSettings
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Preferences

The settings of MedReminder on this PC that are not a profile's own. The reference country is shared by every profile and, with a shared installation, by every device.

## Information kept

- **Interface language** — English, Italian, French, Spanish or German; e-mails and the therapy report use it too
- **Reference country** — the country whose catalogue is used (Italy by default)
- **Check for app and catalogue updates** — whether MedReminder checks for a new version and refreshes the catalogues
- **Log database queries** — diagnostic logging of database commands, without values
- **Start at Windows login** — whether MedReminder starts hidden in the notification area
- **New medicine warning** — the warning threshold and channels a new medicine starts with, as chosen in the guided setup
- **Default dose times** — the times of the doses of medicines without slots, by number of administrations per day
