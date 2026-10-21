---
availability:
  - place: desktop::profile
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

# Warn when stock runs low

Every 30 minutes, warn once through a medicine's channels when its days left reach the warning threshold, and once more at half of it if no stock was added meanwhile.
