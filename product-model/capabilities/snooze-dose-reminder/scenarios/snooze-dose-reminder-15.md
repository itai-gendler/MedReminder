---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Remind me in 15 minutes on the dose reminder
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product schedules the same reminder with Windows for 15 minutes later
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Notifications/ToastActivationRouter.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Notifications/NotificationActions.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Remind me in 15 minutes

## Trigger

The person cannot take the dose right now.

## Outcome

The reminder is shown again 15 minutes later, even if MedReminder was closed meanwhile; nothing is recorded.

## Edge cases

- Intakes are not recorded from the notification: Register intake is used for that.
