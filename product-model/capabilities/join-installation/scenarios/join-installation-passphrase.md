---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: >-
      Choose Join an existing installation…, With the installation passphrase, select the storage and type the
      passphrase
    kind: actor
    actor: administrator
    entities:
      - entity: installation
        effect: reads
        facts:
          - Storage
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: >-
      As an administrator of the installation, choose the own profile, type its PIN and select the profiles
      for this PC
    kind: actor
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::administration::installation
  - text: The Product joins and restarts with the chosen profiles and the installation's settings
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
    contexts:
      desktop:
        place: desktop::administration::installation
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

# Join with the installation passphrase

## Trigger

No other device of the installation is at hand.

## Outcome

After an administrator of the installation approves with their profile's PIN and chooses the profiles, this PC joins with them.

## Edge cases

- Profiles already on the new PC are added to the installation.
- A profile whose sync group belongs to another installation cannot be added.
