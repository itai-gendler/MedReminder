---
entities:
  - entity: installation
    shows:
      - Storage
      - Master device
    collects:
      - Storage
      - Installation passphrase
  - entity: device
    shows:
      - Name
      - Last seen
      - Role
      - Profiles held
    collects:
      - Profiles held
  - entity: profile
    shows:
      - Name
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HouseholdDialog.cs#HouseholdDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HouseholdProfilesDialog.cs
---

# Installation window

The shared installation: its status (storage, master and any action needed) and its devices with their role.
