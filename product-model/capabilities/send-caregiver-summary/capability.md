---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Notifications/CaregiverDigest.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Send the caregiver a weekly summary

Every 7 days, e-mail the caregiver alone a summary of the profile's stock, without anything about the doses taken.
