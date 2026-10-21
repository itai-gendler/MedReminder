---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The periodic check runs, every 30 minutes while MedReminder is running
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: >-
      An active medicine's days left reached its warning threshold, and no warning was given since stock was
      last added
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Days left
          - Warning threshold
          - Therapy end date
          - Run-out date
          - Notification channels
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product shows a Windows notification with the medicine and its days left
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Days left
    contexts:
      desktop:
        place: desktop::profile
  - text: >-
      The Product e-mails the recipient, copying the caregiver, with the stock, the days left and the run-out
      date as a calendar file
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Stock
          - Days left
          - Run-out date
      - entity: profile
        effect: reads
        facts:
          - Recipient
          - Caregiver e-mail
          - Copy to the caregiver
      - entity: email-account
        effect: reads
        facts:
          - Host
          - Sender
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

# Warn at the warning threshold

## Trigger

The periodic check finds an active medicine whose days left reached its warning threshold.

## Outcome

One Windows notification and one e-mail, with the run-out date as a calendar file, tell the person to ask for a new prescription; the caregiver gets a copy if they asked for it.

## Edge cases

- The check uses the recorded stock, not the list's intraday estimate: on the day the threshold is crossed, the list can show Warning a few hours before the warning.
- With a repeatable prescription that still has dispensations, the warning says how many are left and until when, instead of suggesting a new prescription.
- Clicking the notification opens MedReminder on the medicine.
