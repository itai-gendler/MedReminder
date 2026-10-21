---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: >-
      Set Schedule to Advanced and choose the regime type: weekly pattern, cyclic, linear or stepped tapering,
      or as needed
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Schedule
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: Add administration times, each with a dose, an optional time or moment and a label
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Administration times
      - entity: moment
        effect: reads
        facts:
          - Name
          - Time
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
      The Product records the medicine with its regime, the administration times placing each day's quantity
      in the day
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
    contexts:
      desktop:
        place: desktop::profile::medicine
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

# Add a medicine with an advanced regime

## Trigger

The therapy does not use the same amount every day: a weekly pattern, a cycle, a taper or doses taken only as needed.

## Outcome

The medicine is deducted day by day as the regime says; as-needed medicines track the stock without a run-out date.

## Decision points

### How the day's quantity is placed

Do the administration times set the quantity, or only place it?

- Simple schedule with administration times → the day's quantity is the sum of the doses of the slots not taken as needed
- Advanced regime with administration times → the regime sets the day's quantity and the slots split it in proportion to their doses

## Edge cases

- A slot marked as needed is never deducted automatically and is not part of the daily total; when every slot is as needed, the medicine behaves as an as-needed one.
- On a cyclic pause day nothing is due and no dose reminder is shown.
- With Advanced, the dose fields at the top of the window are not used.
