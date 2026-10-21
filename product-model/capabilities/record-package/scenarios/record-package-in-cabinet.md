---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine and open its boxes from the Stock menu
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: >-
      Click New…, enter the quantity, the printed expiry and, optionally, the days of use after opening, the
      opening date and the batch
    kind: actor
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - Quantity
          - Expiry
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: The Product records the box without changing the stock
    kind: product
    actor: user
    entities:
      - entity: package
        effect: creates
        to: Unopened
        facts:
          - Quantity
          - Expiry
          - Use within
          - Batch
    contexts:
      desktop:
        place: desktop::profile::packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#SaveStockPackage
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackageEditDialog.cs#PackageEditDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Record a box already in the cabinet

## Trigger

The person starts following expiry for boxes bought before.

## Outcome

The box is listed with its status and use-until date; the stock is unchanged.

## Edge cases

- A box recorded with an opening date starts Opened.
- A printed expiry more than 20 years from today is refused as a typing or reading error.
