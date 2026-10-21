---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Therapy → New medicine
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: >-
      Type the name and fill in the unit, dose per administration, administrations per day, therapy start date
      and warning threshold
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Unit
          - Schedule
          - Therapy start date
          - Warning threshold
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: Enter the initial stock quantity and tick the notification channels
    kind: actor
    actor: user
    entities:
      - entity: stock-movement
        effect: reads
        facts: []
      - entity: medicine
        effect: reads
        facts:
          - Notification channels
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: Save
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: >-
      The Product records the medicine as active, with a first schedule version from the start date and the
      initial stock as its first stock movement
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: creates
        to: Active
        facts:
          - Name
          - Active ingredient
          - Package
          - Product code
          - Unit
          - Schedule
          - Administration times
          - Therapy start date
          - Therapy end date
          - Warning threshold
          - Notification channels
          - Remind at dose time
          - Reference doctor
          - Notes
      - entity: stock-movement
        with: medicine
        effect: creates
        facts:
          - Type
          - Quantity
          - Day
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: The Product shows the new medicine in the list with its stock, days left and run-out date
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Stock
          - Days left
          - Run-out date
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/AddMedicine.cs#AddMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MedicineEditDialog.cs#MedicineEditDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/SearchCatalogueUseCase.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/CatalogueEntryFill.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Add a medicine with a fixed daily dose

## Trigger

The person starts a therapy, or starts following one in MedReminder.

## Outcome

The medicine is in the list with its stock, days left and estimated run-out date, and is deducted every day from its start date.

## Edge cases

- Without an initial quantity no stock movement is recorded, and the medicine starts empty.
- The warning threshold and channels start from the values the guided setup chose on this PC.
