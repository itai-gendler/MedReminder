---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Type part of the name or of the active ingredient
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: The Product suggests up to 20 catalogue medicines, marking suspended or withdrawn products
    kind: product
    actor: user
    entities:
      - entity: catalogue-medicine
        effect: reads
        facts:
          - Name
          - Active ingredient
          - Package
          - Marketing state
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: Pick the one that matches the box
    kind: actor
    actor: user
    entities:
      - entity: catalogue-medicine
        effect: reads
        facts:
          - Name
          - Package
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: >-
      The Product fills in the name with strength and form, the active ingredient, the pack description, the
      unit and the product code
    kind: product
    actor: user
    entities:
      - entity: catalogue-medicine
        effect: reads
        facts:
          - Name
          - Active ingredient
          - Package
          - Product code
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: Complete the therapy and save
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Schedule
          - Therapy start date
          - Warning threshold
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: The Product records the medicine linked to the catalogue
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

# Fill in a medicine from the catalogue

## Trigger

The person starts typing the name or the active ingredient of a medicine the reference catalogue lists.

## Outcome

The medicine is recorded with the catalogue's name, active ingredient, package, unit and product code, so its leaflet, shortage state and equivalents can be found.

## Edge cases

- A medicine may appear twice, national and EU: the person picks the one that matches the box.
- Editing the name or active ingredient after picking drops the catalogue link.
- A medicine not in the list is saved by its typed name; it works the same, without the catalogue link.
