---
appliesTo:
  - type: capability
    id: refresh-catalogue
  - type: capability
    id: check-for-updates
  - type: entity
    id: preferences
    facts:
      - Check for app and catalogue updates
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/CatalogueRefreshHostedService.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Catalogues and new versions are checked only with the update check on

Check for app and catalogue updates governs both the version check and the catalogue refresh; a new version is never downloaded or installed automatically.
