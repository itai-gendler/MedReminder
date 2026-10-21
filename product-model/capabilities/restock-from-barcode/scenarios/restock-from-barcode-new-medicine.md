---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Scan the box
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product finds no medicine with the code and finds it in the catalogue
    kind: product
    actor: user
    entities:
      - entity: catalogue-medicine
        effect: reads
        facts:
          - Name
          - Product code
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: Choose to add it as new
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product opens the new-medicine window filled in from the catalogue
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
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/RestockByScanQuery.cs#RestockByScanQuery
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/LinkMedicineToReferenceUseCase.cs#LinkMedicineToReferenceUseCase
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MedicinePickerDialog.cs#MedicinePickerDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Add a scanned medicine that is not in the list

## Trigger

The scanned code belongs to no medicine of the profile and the person starts a new therapy with it.

## Outcome

The new-medicine window opens filled in from the catalogue, with its own initial stock, so no restock follows.
