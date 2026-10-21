---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: MedReminder starts, or 24 hours passed since the last refresh
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: >-
      Check for app and catalogue updates is on and the published catalogue is newer than the one the profile
      has
    kind: condition
    entities:
      - entity: preferences
        effect: reads
        facts:
          - Check for app and catalogue updates
          - Reference country
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product imports the catalogue of the reference country and the EU one into the open profile
    kind: product
    entities:
      - entity: catalogue-medicine
        effect: changes
        facts:
          - Name
          - Active ingredient
          - Package
          - Product code
          - Marketing state
          - Information links
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile
  - text: >-
      With Italy, the Product also downloads the AIFA shortage and transparency lists and the list of regional
      health services
    kind: product
    entities:
      - entity: shortage
        effect: changes
        facts:
          - Product code
          - Start
          - Expected end
          - Reason
          - Equivalents reported
          - List date
      - entity: equivalence-group
        effect: changes
        facts:
          - Group
          - Reference price
          - Packages
          - Note
    contexts:
      desktop:
        place: desktop::profile
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

# Refresh the catalogue of the reference country

## Trigger

MedReminder starts, or a day passed since the last refresh.

## Outcome

The open profile has the latest monthly catalogue; for Italy the shortage and transparency lists are current too.

## Edge cases

- Without an internet connection nothing changes.
- With several profiles, only the open one is updated; the others the first time they are opened.
- With EU as reference country the list holds only the EU medicines.
