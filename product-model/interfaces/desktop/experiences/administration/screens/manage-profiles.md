---
entities:
  - entity: profile
    shows:
      - Name
      - Last used
    collects:
      - Name
      - PIN
  - administrator
  - user
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ProfilesManagerForm.cs#ProfilesManagerForm
---

# Manage profiles

The profiles on this PC with their role, whether they have a PIN, when each was last used and which one is open.
