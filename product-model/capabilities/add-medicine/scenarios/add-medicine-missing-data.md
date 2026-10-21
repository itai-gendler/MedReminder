---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Save with the name, unit, dose, administrations per day, start date or warning threshold missing
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Unit
          - Schedule
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: A required field is empty, or the therapy end date is before the start date
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Therapy start date
          - Therapy end date
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: The Product keeps the window open and names the missing or inconsistent data
    kind: product
    actor: user
    entities: []
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

# Refuse a medicine with missing data

## Trigger

The person saves without a required field, or with an end date before the start date.

## Outcome

Nothing is recorded and the window says what is missing or inconsistent.
