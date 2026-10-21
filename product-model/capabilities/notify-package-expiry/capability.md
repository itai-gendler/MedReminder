---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageExpiryNotices.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/PackageExpiryRules.cs#PackageExpiryRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Notify package expiry

Give an "expires soon" notice before a box expires and an "expired" notice the day after, once each, on the medicine's channels, even when the medicine is no longer in use.
