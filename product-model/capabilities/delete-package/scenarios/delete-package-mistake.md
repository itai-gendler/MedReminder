---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the box, click Delete and confirm
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
  - text: The Product removes the box
    kind: product
    actor: user
    entities:
      - entity: package
        effect: removes
        from: Unopened
    contexts:
      desktop:
        place: desktop::profile::packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageUseCases.cs#DeleteStockPackage
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackagesDialog.cs#PackagesDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Delete a box entered by mistake

## Trigger

A box was recorded twice.

## Outcome

The box is gone and the stock is unchanged.
