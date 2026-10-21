---
appliesTo:
  - type: entity
    id: preferences
    effect: changes
    facts:
      - Reference country
      - Log database queries
permits:
  - actors:
      - administrator
  - unattended: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/InstallationSettingsUseCases.cs#UpdateGeneralSettings
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only an administrator changes the reference country and database-query logging

The reference country applies to every profile and, with a shared installation, to every device; database-query logging is a diagnostic of this PC. A user sees both but cannot change them.
