---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: The periodic check runs on a device that is not the master
    kind: condition
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
        place: desktop::profile
    unattended: true
  - text: A medicine reached its warning threshold
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Days left
          - Warning threshold
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product shows the Windows notification and leaves the e-mail to the master device
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Days left
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/MedicationMonitor.cs#MedicationMonitor
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/NotificationCycle.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Notifications/NotificationTexts.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Notifications/LowStockEmail.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/MedicationMonitorHostedService.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Leave the e-mail to the master device

## Trigger

The profile is held by a device of a shared installation that is not the master.

## Outcome

This device shows its own Windows notification; the master device sends the e-mail, once for every device.

## Edge cases

- In a synced profile without a master, a low-stock e-mail is sent once per group; two PCs that check before they have synced may both send.
