---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs#UpdateNotificationSettings
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs#UpdateProfileRegion
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SettingsDialog.Notifications.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Email/NotificationRecipientRules.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Change Notifications settings

Set, for the open profile, who receives its e-mails, whether a caregiver gets copies and the weekly summary, the doctor's address for requests, when package expiry notices come, and the region.
