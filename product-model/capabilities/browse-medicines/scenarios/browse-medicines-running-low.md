---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The Product counts the active medicines that are empty, running low or suspended, and all of them
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Click the Running low box
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product lists only the medicines in warning, with their stock, days left and run-out date
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Stock
          - Days left
          - Run-out date
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Overview/MedicineListFilter.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Overview/MedicineOverviewLoader.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/IntradayConsumption.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Show only the medicines running low

## Trigger

The person wants to see which medicines need a new prescription soon.

## Outcome

Only the medicines below their warning threshold are listed; clicking the box again shows all of them.

## Edge cases

- During the day the stock and the days left already leave out today's doses whose time has passed; hovering over the stock shows the start-of-day value.
- A slot with a typed description and no time is counted at the end of the day.
