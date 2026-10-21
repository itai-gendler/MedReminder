---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click the leaflet link in the Information row
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Product code
    contexts:
      desktop:
        place: desktop::profile::medicine
  - text: The Product opens the link the catalogue holds for the product code
    kind: product
    actor: user
    entities:
      - entity: catalogue-medicine
        effect: reads
        facts:
          - Information links
    contexts:
      desktop:
        place: desktop::profile::medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/MedicineInfoLink.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MedicineEditDialog.cs#MedicineEditDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Open the package leaflet

## Trigger

The person wants to read the leaflet of an Italian medicine.

## Outcome

The AIFA document opens in the browser.

## Edge cases

- The leaflet and summary links appear with Italy as reference country, when the catalogue has them; with the United States, the leaflet opens the label on DailyMed.
