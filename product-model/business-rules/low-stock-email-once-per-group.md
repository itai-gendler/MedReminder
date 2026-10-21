---
appliesTo:
  - type: capability
    id: warn-low-stock
  - type: entity
    id: sync-group
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/NotificationCycle.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/MedicationMonitor.cs#MedicationMonitor
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A low-stock e-mail is sent once per sync group

Every device of a synced profile records the low-stock e-mails it sent, so the others leave the e-mail out and show only their own notification.
