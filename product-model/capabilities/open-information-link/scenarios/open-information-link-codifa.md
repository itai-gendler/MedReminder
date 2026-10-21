---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Right-click the medicine and choose Open Codifa page
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product opens the Codifa page of the product code in the browser
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Product code
    contexts:
      desktop:
        place: desktop::profile::medicines
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

# Open the Codifa page

## Trigger

The person wants the composition, class, dispensing and price of an Italian package.

## Outcome

The public Codifa page of the package opens in the browser, whatever the reference country.
