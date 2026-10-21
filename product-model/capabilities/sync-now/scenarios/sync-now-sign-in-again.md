---
kind: edge
routes:
  desktop: Desktop
steps:
  - text: The cloud session of the group has ended
    kind: condition
    entities:
      - entity: sync-group
        effect: reads
        facts:
          - Storage
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: Click Sign in to OneDrive again (or Google Drive) and sign in with the same account
    kind: actor
    actor: administrator
    entities: []
    contexts:
      desktop:
        place: desktop::administration::sync
  - text: The Product resumes syncing and sends the changes made meanwhile
    kind: product
    actor: administrator
    entities:
      - entity: sync-group
        effect: changes
        facts:
          - Last sync
    contexts:
      desktop:
        place: desktop::administration::sync
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncEngine.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/ApplyRemoteOperations.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/SyncHostedService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Sync/SyncGroupUseCases.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/SyncDialog.cs#SyncDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure/Sync/SyncSetupService.cs
  - kind: doc
    role: intent
    target: docs/SYNC-FORMAT.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Sign in again after the session ended

## Trigger

The OneDrive or Google Drive session ended, after a password change or a long inactivity.

## Outcome

After signing in again the changes made meanwhile are sent; nothing is lost.

## Edge cases

- Signing in with a different account is refused.
