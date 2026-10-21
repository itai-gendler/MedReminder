---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/RemoteCatalogueRefresher.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Hosting/CatalogueRefreshHostedService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/ShortageFeed.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/EquivalenceFeed.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/RegionalServicesFeed.cs
  - kind: doc
    role: intent
    target: docs/CATALOGUE-DATA.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Refresh the catalogue

At startup and once a day while MedReminder stays open, with the update check on, download the monthly catalogue of the reference country and the EU one when newer, and for Italy the shortage list, the transparency list and the regional prescription services. Only data is downloaded; nothing is executed.
