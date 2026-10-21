---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the box, click Edit…, correct the expiry and save
    kind: actor
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - Expiry
          - Use within
          - Batch
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: The Product saves the box
    kind: product
    actor: user
    entities:
      - entity: package
        effect: changes
        facts:
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
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Correct the expiry of a box

## Trigger

The expiry was read wrongly.

## Outcome

The box's use-until date and status follow the corrected expiry; the stock is unchanged.
