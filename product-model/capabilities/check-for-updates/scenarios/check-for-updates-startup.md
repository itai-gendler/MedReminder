---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: MedReminder starts
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: Check for app and catalogue updates is on
    kind: condition
    entities:
      - entity: preferences
        effect: reads
        facts:
          - Check for app and catalogue updates
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product checks the latest release and tells the person when a newer version exists
    kind: product
    entities: []
    contexts:
      desktop:
        place: desktop::profile
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

# Check for a new version at startup

## Trigger

MedReminder starts with the update check on.

## Outcome

A notice appears only when a newer version exists.
