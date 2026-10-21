---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Scan barcode…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: Scan the box, type the code printed under it, or hold the box in front of the webcam
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product reads the product code and looks it up in the catalogue
    kind: product
    actor: user
    entities:
      - entity: catalogue-medicine
        effect: reads
        facts:
          - Product code
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
  - text: The Product fills in the medicine from the catalogue medicine found
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

# Fill in a medicine by scanning the box

## Trigger

The person has the box at hand and a USB scanner or a webcam.

## Outcome

A code found in the catalogue fills in the medicine as a catalogue pick does; any other code changes nothing.

## Edge cases

- On Italian boxes the code to scan is the AIC barcode; a DataMatrix code needs a 2D scanner and is often not in the catalogue.
- A code that is not in the catalogue is shown, and nothing in the form changes.
- The webcam turns off when a code is read, when the person switches back to the scanner, closes the window, or after 30 seconds; no image is saved or sent.
