---
appliesTo:
  - type: capability
    id: send-caregiver-summary
  - type: entity
    id: profile
    facts:
      - Weekly stock summary
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Notifications/CaregiverDigest.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# The caregiver's weekly summary says nothing about doses

It holds the stock, status and run-out date of each active medicine and the boxes expired or expiring soon, and is sent once per profile even when the profile is synced on several PCs.
