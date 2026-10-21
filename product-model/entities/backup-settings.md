---
singleton: true
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Abstractions/BackupSettings.cs#BackupSettings
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Backup settings

How MedReminder backs up every profile: a daily copy to a local folder, and an optional encrypted daily copy to the cloud.

## Information kept

- **Daily automatic backup** — on or off
- **Backup folder** — where the daily copies go, ideally an external disk
- **Preferred time** — when the daily copy is made
- **Retention** — days the copies are kept; the newest copy of each profile is always kept
- **Cloud backup** — on or off
- **Storage** — OneDrive, Google Drive, or a folder a cloud client synchronizes
- **Snapshots to keep** — how many encrypted copies stay in the cloud (30 by default)
- **Backup passphrase** — at least 12 characters; stays on this PC and is never sent
