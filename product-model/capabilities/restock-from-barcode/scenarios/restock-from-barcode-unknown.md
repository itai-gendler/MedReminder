---
kind: validation
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
  - text: The code carries no national product code, or neither the profile nor the catalogue knows it
    kind: condition
    entities:
      - entity: catalogue-medicine
        effect: reads
        facts:
          - Product code
      - entity: profile
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product shows the code read and changes nothing
    kind: product
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
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

# Refuse a code that identifies no medicine

## Trigger

The scanned code is in neither the profile nor the catalogue, or is only a GTIN without a national code.

## Outcome

Nothing changes; the person is told the code read.
