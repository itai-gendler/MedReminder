---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The periodic check runs
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: >-
      A box the stock still covers is within the profile's notice days of its use-until date (30 before a
      printed expiry, 3 before the end of the period after opening, by default)
    kind: condition
    entities:
      - entity: package
        effect: reads
        facts:
          - Use until
          - In stock
      - entity: profile
        effect: reads
        facts:
          - Package expiry notice
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product gives the expires-soon notice on the medicine's channels
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Notification channels
      - entity: package
        effect: reads
        facts:
          - Use until
    contexts:
      desktop:
        place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageExpiryNotices.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/PackageExpiryRules.cs#PackageExpiryRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Notify a box that expires soon

## Trigger

A box in stock reaches the notice period before its use-until date.

## Outcome

One notice per box tells the person to use it first or check it; clicking it opens the medicine's packages.

## Edge cases

- With a notice period of 0 only the expired notice is left.
- One e-mail per check gathers every medicine's boxes; a failed e-mail is retried on the next check without showing the notification again.
