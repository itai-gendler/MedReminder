---
entities:
  - entity: profile
    shows:
      - Recipient
      - Caregiver e-mail
      - Copy to the caregiver
      - Weekly stock summary
      - Doctor e-mail
      - Package expiry notice
      - Region
    collects:
      - Recipient
      - Caregiver e-mail
      - Copy to the caregiver
      - Weekly stock summary
      - Doctor e-mail
      - Package expiry notice
      - Region
      - PIN
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Notifications.cs
---

# Notifications settings

The Notifications section of Settings, for the open profile: the recipient, the caregiver and what they receive, the doctor's address, the package expiry notice, the region, and the profile's own PIN.
