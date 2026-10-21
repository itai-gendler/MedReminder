---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Save a box with an inconsistent quantity, period or date
    kind: actor
    actor: user
    entities:
      - entity: package
        effect: reads
        facts:
          - Quantity
          - Use within
          - Opened on
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: The box fails a check
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile::packages
  - text: The Product names the problem and records nothing
    kind: product
    actor: user
    entities: []
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

# Refuse an inconsistent box

## Trigger

The quantity is zero, the days after opening are out of 1 to 365, or a date is in the future.

## Outcome

Nothing is recorded and the window says what to fix.
