---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The Product opens the guided setup by itself, since the profile has no medicine
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::guided-setup
  - text: Answer that the medicines are for someone the person looks after
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::guided-setup
  - text: Add the first medicines with Add a medicine…
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Days left
    contexts:
      desktop:
        place: desktop::profile::guided-setup
  - text: Choose how many days before running out to warn (7, 10, 14 or another value) and how
    kind: actor
    actor: user
    entities:
      - entity: preferences
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::guided-setup
  - text: >-
      The Product applies the choice to the medicines added in the setup and keeps it as the starting value of
      new medicines on this PC
    kind: product
    actor: user
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
  - text: Enter the address of the person looked after and the carer's own address
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Recipient
          - Caregiver e-mail
    contexts:
      desktop:
        place: desktop::profile::guided-setup
  - text: >-
      The Product saves the warnings to that address and the copies to the carer, and summarises what
      MedReminder will do
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: changes
        facts:
          - Recipient
          - Caregiver e-mail
          - Copy to the caregiver
          - Weekly stock summary
    contexts:
      desktop:
        place: desktop::profile::guided-setup
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/GuidedSetupForm.cs#GuidedSetupForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/GuidedSetup/GuidedSetupFlow.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/GuidedSetup/GuidedSetupUseCases.cs#ApplyGuidedSetupWarning
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Set up the medicines of someone looked after

## Trigger

A profile with no medicine is opened on this PC for the first time.

## Outcome

The medicines added warn with the chosen lead time and channels, new medicines on this PC start with them, and the warnings go to the person looked after, with copies to the carer.

## Edge cases

- Medicines already in the list keep their threshold and channels.
- If the person looked after has no address, the warnings come to the carer.
- When e-mail is the only channel and no e-mail can be sent yet, the summary offers Also warn on Windows.
- Not now, Esc or closing the window ends it, and it does not open by itself again for that profile on this PC; it stays in ? → Guided setup… .
