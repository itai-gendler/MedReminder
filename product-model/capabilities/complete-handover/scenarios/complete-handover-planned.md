---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The Product opens the master handover by itself, with the installation's settings
    kind: product
    actor: administrator
    entities:
      - entity: email-account
        effect: reads
        facts:
          - Host
          - Port
          - Sender
      - entity: backup-settings
        effect: reads
        facts:
          - Cloud backup
          - Storage
          - Snapshots to keep
      - entity: preferences
        effect: reads
        facts:
          - Reference country
    contexts:
      desktop:
        place: desktop::administration::master-handover
  - text: Test the e-mail connection from this PC
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
  - text: Sign in to the cloud backup storage with the same account and type the cloud backup passphrase again
    kind: actor
    actor: administrator
    entities:
      - entity: backup-settings
        effect: reads
        facts:
          - Storage
    contexts:
      desktop:
        place: desktop::administration::master-handover
  - text: Optionally type the installation passphrase, and click Confirm
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::administration::master-handover
  - text: The Product records the handover; the device takes over when the old master hands over
    kind: product
    actor: administrator
    entities:
      - entity: backup-settings
        effect: changes
        facts:
          - Backup passphrase
      - entity: installation
        effect: changes
        facts:
          - Master device
      - entity: device
        effect: reads
        facts: []
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

# Complete a planned handover

## Trigger

An administrator profile opens on the device that was elected master.

## Outcome

The device becomes the master once the old master hands over at its next sync; new profiles appear at the next start.

## Edge cases

- The cloud backup passphrase is never copied between devices.
- Later postpones the handover; it stays in Tools → Installation… → Complete the handover….
