---
appliesTo:
  - type: capability
    id: warn-low-stock
  - type: capability
    id: remind-dose-time
  - type: capability
    id: remind-prescription-to-collect
  - type: capability
    id: remind-deadline
  - type: capability
    id: notify-package-expiry
  - type: capability
    id: notify-shortage
  - type: capability
    id: send-caregiver-summary
  - type: capability
    id: request-prescription
  - type: capability
    id: back-up-every-profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Household/MasterRules.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Household/HouseholdMasterRole.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# In a shared installation only the master device sends e-mail

The master sends every e-mail of every profile it holds and makes the cloud backup; the other devices show their reminders on screen only, so each e-mail arrives once. A master that has not synced the installation for 24 hours stops sending until it syncs again.
