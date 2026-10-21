---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: The periodic check runs
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: The days left reached half of the warning threshold and no stock was added since the first warning
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
  - text: The Product gives the second warning on the medicine's channels
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Days left
          - Run-out date
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

# Warn again at half of the threshold

## Trigger

The person ignored the first warning and added no stock.

## Outcome

A second warning follows on the same channels; with a threshold of 10 days, the first comes at 10 days left and the second at 5.

## Edge cases

- A medicine already below half when first checked gets only the second warning.
