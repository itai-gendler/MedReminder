---
kind: primary
result: achieved
routes:
  desktop: Desktop
steps:
  - text: Choose Create profile and type the name
    kind: actor
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::welcome
    capability: create-profile
  - text: The Product creates the first profile as an administrator
    kind: product
    actor: administrator
    entities:
      - entity: profile
        with: administrator
        effect: creates
        facts:
          - Name
          - PIN
      - entity: administrator
        effect: creates
        facts: []
    contexts:
      desktop:
        place: desktop::profile::welcome
    capability: create-profile
  - text: The Product opens the main window and, since the profile has no medicine, the guided setup by itself
    kind: product
    actor: administrator
    entities:
      - entity: profile
        effect: reads
        facts:
          - Name
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::guided-setup
    capability: create-profile
  - text: Add the first medicines and choose how many days before running out to warn, and how
    kind: actor
    actor: administrator
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Days left
      - entity: preferences
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::guided-setup
    capability: complete-guided-setup
  - text: The Product applies the choice to the medicines added and keeps it for new medicines on this PC
    kind: product
    actor: administrator
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Warning threshold
          - Notification channels
      - entity: preferences
        effect: changes
        facts:
          - New medicine warning
    contexts:
      desktop:
        place: desktop::profile::guided-setup
    capability: complete-guided-setup
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/FirstRunWizardForm.cs#FirstRunWizardForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/GuidedSetupForm.cs#GuidedSetupForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Create the first profile and set up its warnings

## Trigger

MedReminder is launched on a PC where it was never used.

## Outcome

The person has an administrator profile whose first medicines warn with the lead time and channels they chose.
