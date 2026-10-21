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
  - text: The Product finds no medicine with the code, finds it in the catalogue and offers to add it
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
    capability: restock-from-barcode
  - text: Choose to add it as new
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::scan-barcode
    capability: restock-from-barcode
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
    capability: restock-from-barcode
  - text: Complete the therapy and the initial stock and save
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
    capability: add-medicine
  - text: The Product records the medicine
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
    capability: add-medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Scan an unknown box and add it

## Trigger

The person scans a box that no medicine of the profile carries.

## Outcome

The new medicine is recorded with the catalogue's name, package and code and its own initial stock.
