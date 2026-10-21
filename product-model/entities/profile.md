---
relations:
  - entity: medicine
    verb: owns
    cardinality: one-to-many
  - entity: deadline
    verb: owns
    cardinality: one-to-many
  - entity: sync-group
    verb: has
    cardinality: one-to-one
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Profiles/ProfileRegistry.cs#ProfileRegistry
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Abstractions/NotificationSettings.cs#NotificationSettings
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Settings/ProfileUiSettingsFile.cs#ProfileUiSettingsFile
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Profile

One person whose medicines MedReminder follows, such as yourself or a parent you look after. Each profile has its own medicines and its own recipients, and only one profile is open at a time.

## Information kept

- **Name** — the name shown for the profile, in the title and status bar and in the profile lists
- **PIN** — optional; asked when the profile is opened, to avoid opening the wrong profile by accident
- **Recipient** — the address that receives this profile's e-mails
- **Caregiver e-mail** — optional second address, different from the recipient, that receives copies
- **Copy to the caregiver** — which kinds of e-mail the caregiver gets: low-stock warnings, dose reminders, prescription and deadline reminders, shortage notices, package expiry notices (all of them until changed)
- **Weekly stock summary** — whether the caregiver also gets a summary every 7 days, and the day it was last sent
- **Doctor e-mail** — optional; used only for prescription requests sent from MedReminder
- **Package expiry notice** — days before the printed expiry (default 30) and before the end of the period after opening (default 3) when the "expires soon" notice comes
- **Region** — the Italian region or autonomous province whose prescription service the profile opens
- **Text size** — Normal, Large or Extra large
- **Appearance** — Same as Windows, Light or Dark
- **Last used** — when the profile was last opened; the last used one opens at Windows startup
