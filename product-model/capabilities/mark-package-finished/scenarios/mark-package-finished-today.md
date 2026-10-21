---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the box in use and click Finished
    kind: actor
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - Status
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: The Product closes the box as Finished today
    kind: product
    actor: user
    entities:
      - entity: package
        effect: changes
        from: Opened
        to: Finished
        facts:
          - Closed on
          - Status
    contexts:
      desktop:
        place: desktop::profile::packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#SaveStockPackage
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Finish a box

## Trigger

The last tablet of a box was taken.

## Outcome

The box is closed as Finished today and gives no more notices; the stock does not change.

## Edge cases

- An unopened box can be marked finished too.
- A box the stock no longer covers is Used up by itself, without being marked.
