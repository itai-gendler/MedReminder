---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UpdateChecking/UpdateCheck.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/UpdateChecking/GitHubUpdateChecker.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Check for updates

Look for a new MedReminder version on GitHub Releases, at startup when the update check is on, or on request. A new version is never downloaded or installed automatically.
