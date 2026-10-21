---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select an unopened box and click Opened today
    kind: actor
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - Status
          - Expiry
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: The Product records today as the opening date; the stock does not change
    kind: product
    actor: user
    entities:
      - entity: package
        effect: changes
        from: Unopened
        to: Opened
        facts:
          - Opened on
          - Use until
          - In stock
    contexts:
      desktop:
        place: desktop::profile::packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/PackageAllocation.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Open a box today

## Trigger

The person opens a new bottle of eye drops.

## Outcome

The box is opened today; its use-until date is the earlier of the printed expiry and the last day of use after opening, the opening day counting as day 1.

## Edge cases

- Opened on 1 March with 28 days of use: use until 28 March.
