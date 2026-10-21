---
entities:
  - entity: preferences
    shows:
      - Interface language
      - Reference country
      - Check for app and catalogue updates
      - Log database queries
    collects:
      - Interface language
      - Reference country
      - Check for app and catalogue updates
      - Log database queries
  - entity: profile
    shows:
      - Text size
      - Appearance
    collects:
      - Text size
      - Appearance
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.General.cs
---

# General settings

The General section of Settings: interface language, text size and appearance of this profile, the update check, and, for administrators, the reference country and database-query logging.
