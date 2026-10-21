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
  - text: The Product finds several medicines with the scanned code and lists them
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Product code
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: Pick the medicine
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
  - text: The Product opens the stock window on it, set to New package
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

# Choose among medicines with the same code

## Trigger

Two medicines of the profile carry the scanned code.

## Outcome

The stock window opens on the medicine the person picked.
