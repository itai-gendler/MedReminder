---
scope: >-
  The MedReminder Windows desktop app: medicines, stock, schedules, reminders, prescriptions, deadlines,
  catalogue, profiles, backup, sync and the shared installation.
method: >-
  Static reading of the UI forms, application use cases, domain rules and infrastructure at the pinned commit,
  with the user guide as the lead.
covered:
  - description: >-
      Domain entities and calculations: medicines, schedules, stock ledger, packages, prescriptions,
      deadlines, sync and household rules.
    paths:
      - src/MedReminder.Domain/
  - description: Application use cases, monitors, reminders, queries and sync and household orchestration.
    paths:
      - src/MedReminder.Application/
  - description: Desktop windows, notification-area icon, Windows notifications and background schedulers.
    paths:
      - src/MedReminder.UI/
  - description: Persistence, catalogue import, e-mail, cloud storage, export archives, profiles and sync storage.
    paths:
      - src/MedReminder.Infrastructure.Portable/
      - src/MedReminder.Infrastructure/
exclusions:
  - description: Automated tests and their fixtures.
    paths:
      - tests/
      - scripts/feeds/tests/
  - description: Build, signing, installer and release tooling.
    paths:
      - packaging/
      - .github/
      - release.ps1
      - publish-signed-release.ps1
      - Directory.Build.props
      - assets/build/
  - description: Scripts that publish the remote catalogue feeds and their published data files.
    paths:
      - scripts/
      - data/
  - description: Developer tool that builds the reference catalogue database and its schema.
    paths:
      - src/MedReminder.DataImporter/
      - database/
  - description: Translations, icons and shipped settings files.
    paths:
      - assets/
  - description: Documentation, analysis notes, plans and a presentation.
    paths:
      - docs/
      - deliverables/
unmapped:
  - description: Support Development dialog and its payment-link providers.
    paths:
      - src/MedReminder.Application/Donations/
      - src/MedReminder.Infrastructure/Donations/
      - src/MedReminder.UI/Forms/DonateForm.cs
  - description: Integrated user guide viewer and About window.
    paths:
      - src/MedReminder.UI/Forms/HelpViewerForm.cs
      - src/MedReminder.UI/Forms/HelpGuideMarkdown.cs
      - src/MedReminder.UI/Forms/AboutDialog.cs
  - description: Upgrade of a very old single-database installation into profiles.
    paths:
      - src/MedReminder.Infrastructure/Migration/
  - description: Data migrations that backfill moments and as-needed slots after an upgrade.
    paths:
      - src/MedReminder.Application/Migrations/
limitations: []
---

# Coverage
