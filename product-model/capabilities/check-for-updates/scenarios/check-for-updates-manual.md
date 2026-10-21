---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose ? → Check for updates…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product compares the running version with the latest release and says whether a newer one exists
    kind: product
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
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

# Check for a new version now

## Trigger

The person wants to know whether a newer MedReminder exists.

## Outcome

MedReminder says whether a newer version is published; installing it stays with the person.
