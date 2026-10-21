---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: The reference country changes to the United States
    kind: condition
    entities:
      - entity: preferences
        effect: reads
        facts:
          - Reference country
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: The update check is on and an internet connection is available
    kind: condition
    entities:
      - entity: preferences
        effect: reads
        facts:
          - Check for app and catalogue updates
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product downloads the FDA list into the open profile
    kind: product
    entities:
      - entity: catalogue-medicine
        effect: changes
        facts:
          - Name
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

# Download the United States list

## Trigger

An administrator chooses the United States as reference country.

## Outcome

The FDA National Drug Code list, one row per package, without EU medicines, is downloaded right after the choice; until then the catalogue stays empty.
