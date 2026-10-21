---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Every minute MedReminder looks for timed slots whose time has come
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: >-
      The medicine reminds at dose time, has stock left, is within its therapy and not suspended, and the slot
      was not reminded today
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Remind at dose time
          - Stock
          - Administration times
          - Therapy start date
          - Therapy end date
    contexts:
      desktop:
        place: desktop::profile
  - text: >-
      The Product shows "Time to take" with the medicine and the dose, and e-mails it when the Email channel
      is on and this device sends e-mail
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Administration times
          - Notification channels
      - entity: profile
        effect: reads
        facts:
          - Recipient
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/DoseReminderService.cs#DoseReminderService
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/DoseReminderHostedService.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Remind at a timed slot

## Trigger

The time of a slot of a medicine with Remind at dose time arrives.

## Outcome

A Windows notification, and an e-mail if the medicine uses the Email channel, reminds the person to take the dose; it does not come again that day, even after a restart.

## Edge cases

- If the PC was asleep at that time, the reminder still arrives within 30 minutes; later than that it is skipped.
- With zero stock no dose reminder is sent.
- On the night the clock moves forward, a slot inside the skipped hour does not fire.
- On a cyclic pause day nothing is due and no reminder is shown.
