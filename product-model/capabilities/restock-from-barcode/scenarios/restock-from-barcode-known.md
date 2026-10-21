---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Stock → Restock from barcode…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Scan the box
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product reads the product code and finds the one medicine of the profile that carries it
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Product code
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: >-
      The Product selects the medicine and opens the stock window set to New package, with the quantity of its
      last new package and the scanned expiry and batch
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

# Restock a medicine by scanning its box

## Trigger

The person comes back from the pharmacy with a box of a medicine already in the list.

## Outcome

The stock window opens on that medicine, set to New package with the quantity of its last new package, and a DataMatrix code fills in the expiry and batch.
