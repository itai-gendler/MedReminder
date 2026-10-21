---
entities:
  - entity: email-account
    shows:
      - Host
      - Port
      - Sender
  - entity: backup-settings
    shows:
      - Cloud backup
      - Storage
      - Snapshots to keep
    collects:
      - Backup passphrase
  - entity: preferences
    shows:
      - Reference country
  - entity: installation
    shows:
      - Master device
    collects:
      - Installation passphrase
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/HandoverWizardForm.cs#HandoverWizardForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/MasterHandover.cs
---

# Master handover

On a device elected master: the installation's settings, and what the device needs before it takes over: a working e-mail connection, the cloud backup sign-in and passphrase, and the profiles to bring.
