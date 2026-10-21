---
kind: primary
result: achieved
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
    capability: restock-from-barcode
  - text: The Product finds the medicine of the profile that carries the code
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
    capability: restock-from-barcode
  - text: The Product opens the stock window on it, set to New package with the usual quantity
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
    capability: restock-from-barcode
  - text: Check the quantity and expiry and save
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::stock-movement
    capability: add-package
  - text: The Product records the new package and its box
    kind: product
    actor: user
    entities:
      - entity: stock-movement
        effect: creates
        facts:
          - Type
          - Quantity
          - Day
          - Notes
      - entity: package
        effect: creates
        to: Unopened
        facts:
          - Quantity
          - Expiry
          - Use within
          - Batch
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::stock-movement
    capability: add-package
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/RestockByScanQuery.cs#RestockByScanQuery
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Scan a box and save the new package

## Trigger

The person chooses Restock from barcode with the new box at hand.

## Outcome

The medicine's stock includes the new package, with the expiry read from the code when it has one.
