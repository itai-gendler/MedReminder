---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the box and click Discard…
    kind: actor
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - Status
          - In stock
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: The Product proposes the quantity still in the box
    kind: product
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - In stock
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: Confirm or change the quantity left and click Discard
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: The Product closes the box as Discarded and records a negative correction of the quantity left
    kind: product
    actor: user
    entities:
      - entity: package
        effect: changes
        from: Opened
        to: Discarded
        facts:
          - Closed on
          - Status
      - entity: stock-movement
        effect: creates
        facts:
          - Type
          - Quantity
          - Day
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#DiscardStockPackage
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Discard an expired box

## Trigger

A box expired and the person throws it away.

## Outcome

The box is closed as Discarded and the quantity left is removed from the stock.

## Edge cases

- With a quantity left of 0 the box was already empty and the stock does not change.
- A discard by mistake is fixed by deleting the box and adding the units back with a positive correction.
