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
  - text: >-
      The Product finds no medicine with the code, looks it up in the catalogue and offers to add it as a new
      medicine or link it to an existing one
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
  - text: Choose to link it to an existing one
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product lists the medicines of the profile that have no product code yet
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Product code
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: Pick the medicine the box belongs to
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product links the medicine to the scanned product code
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Product code
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product opens the stock window on the linked medicine, set to New package
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Stock
      - entity: package
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::stock-movement
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

# Link a scanned code to a medicine and restock it

## Trigger

The scanned code belongs to no medicine of the profile, but the catalogue knows it and the medicine was entered by name only.

## Outcome

The medicine now carries the scanned product code, so the next scan finds it, and the stock window opens on it set to New package.

## Edge cases

- When every medicine already has a product code, there is nothing to link and the person is told so.
