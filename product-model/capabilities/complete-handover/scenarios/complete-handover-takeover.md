---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: The Product opens the master takeover on the elected device
    kind: product
    actor: administrator
    entities:
      - entity: installation
        effect: reads
        facts:
          - Master device
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::administration::master-handover
  - text: Check the e-mail connection and the cloud backup, then Confirm
    kind: actor
    actor: administrator
    entities:
      - entity: email-account
        effect: reads
        facts:
          - Host
    contexts:
      desktop:
        place: desktop::administration::master-handover
  - text: The Product takes over once the old master has not synced for 25 hours
    kind: product
    actor: administrator
    entities:
      - entity: installation
        effect: changes
        facts:
          - Master device
    contexts:
      desktop:
        place: desktop::administration::master-handover
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/MasterHandover.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HandoverWizardForm.cs#HandoverWizardForm
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

# Take over from a lost master

## Trigger

The master is broken or lost, and another device was made master.

## Outcome

The new master takes over by itself once the old one has been silent for 25 hours.
