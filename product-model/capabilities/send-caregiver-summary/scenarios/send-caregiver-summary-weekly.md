---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The periodic check runs on the device that sends e-mail
    kind: condition
    entities:
      - entity: device
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: The profile has a caregiver who asked for the weekly summary, and 7 days passed since it was last sent
    kind: condition
    entities:
      - entity: profile
        effect: reads
        facts:
          - Caregiver e-mail
          - Weekly stock summary
    contexts:
      desktop:
        place: desktop::profile
  - text: >-
      The Product e-mails the caregiver alone the stock, status and run-out date of each active medicine and
      the expiring boxes
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Stock
          - Status
          - Run-out date
      - entity: package
        effect: reads
        facts:
          - Use until
      - entity: profile
        effect: changes
        facts:
          - Weekly stock summary
    contexts:
      desktop:
        place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Notifications/CaregiverDigest.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Send the weekly stock summary

## Trigger

Seven days passed since the last summary of a profile whose caregiver asked for it.

## Outcome

The caregiver gets the stock, status and run-out date of each active medicine and the boxes expired or expiring soon; the other devices of the profile do not send it again.
