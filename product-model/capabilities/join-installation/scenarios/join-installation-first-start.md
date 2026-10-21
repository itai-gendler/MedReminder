---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Join an existing installation…, then With a code, and type the code shown on the other PC
    kind: actor
    actor: administrator
    entities:
      - entity: installation
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::welcome
  - text: >-
      The Product joins, lists each profile ("added to this device", "already on this device"…) and restarts
      with them
    kind: product
    actor: administrator
    entities:
      - entity: installation
        effect: changes
        from: Not published
        to: Published
        facts:
          - Storage
          - Master device
      - entity: device
        effect: creates
        facts:
          - Name
          - Profiles held
      - entity: profile
        effect: creates
        facts:
          - Name
          - PIN
      - entity: email-account
        effect: changes
        facts:
          - Host
          - Port
          - Use StartTLS
          - Username
          - Password
          - Sender
          - Timeout
      - entity: preferences
        effect: changes
        facts:
          - Reference country
    contexts:
      desktop:
        place: desktop::profile::welcome
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/JoinInstallation.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Household/HouseholdProfileInstaller.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/FirstRunWizardForm.cs#FirstRunWizardForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdSync.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HouseholdDialog.cs#HouseholdDialog
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Join with a code on a new PC

## Trigger

MedReminder starts for the first time on a PC of the family.

## Outcome

MedReminder restarts with the profiles given to this PC and the installation's settings.
