---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Choose to be warned by e-mail
    kind: actor
    actor: user
    entities:
      - entity: preferences
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::guided-setup
  - text: No e-mail account is set up yet
    kind: condition
    entities:
      - entity: email-account
        effect: reads
        facts:
          - Host
    contexts:
      desktop:
        place: desktop::profile::guided-setup
  - text: >-
      The Product tells a user that an administrator has to set up the e-mail account, and offers an
      administrator to open it
    kind: product
    actor: user
    entities:
      - entity: administrator
        effect: reads
        facts: []
      - entity: email-account
        effect: reads
        facts: []
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

# Set up warnings before the e-mail account exists

## Trigger

The person chooses e-mail warnings but no e-mail account is set up.

## Outcome

An administrator can open Set up the e-mail account… from the setup; other profiles are told that an administrator has to do it.
