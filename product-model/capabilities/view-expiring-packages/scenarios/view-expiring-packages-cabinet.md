---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Open the cabinet view of expiring boxes from the Stock menu
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product lists the expired and expiring boxes of every medicine, expired first
    kind: product
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - Status
          - Use until
          - In stock
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::expiring-packages
  - text: Open the packages of one medicine
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
      - entity: package
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::expiring-packages
  - text: The Product opens the boxes of that medicine
    kind: product
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - Status
          - Expiry
          - Opened on
          - Use until
          - In stock
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/ExpiringPackagesQuery.cs#ExpiringPackagesQuery
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/ExpiringPackagesDialog.cs#ExpiringPackagesDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Check the cabinet for expired boxes

## Trigger

The person tidies the medicine cabinet.

## Outcome

The boxes to act on are listed, expired first, and each medicine's boxes open from the list.
