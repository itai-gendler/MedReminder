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
  - text: The days left are within the threshold, but the therapy end date comes before the run-out date
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Days left
          - Therapy end date
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product sends no warning for the medicine
    kind: product
    entities:
      - entity: medicine
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

# Stay silent when the therapy ends first

## Trigger

A medicine runs low, but its therapy ends before the estimated run-out date.

## Outcome

No warning comes: no new prescription is needed.

## Edge cases

- An inactive medicine, a suspended one or one with its channels set to none gets no warning either.
