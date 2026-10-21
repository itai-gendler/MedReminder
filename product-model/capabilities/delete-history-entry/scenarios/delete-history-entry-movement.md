---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine and choose Stock → History…
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product lists the entries of the medicine, newest first, with whether each can be deleted
    kind: product
    actor: user
    entities:
      - entity: stock-movement
        effect: reads
        facts:
          - Type
          - Quantity
          - Day
      - entity: intake
        effect: reads
        facts:
          - Day
          - Status
      - entity: stock-count
        effect: reads
        facts:
          - Day
          - Counted quantity
      - entity: suspension
        effect: reads
        facts:
          - From
          - To
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::history
  - text: Select the mistaken entry, click Delete and confirm
    kind: actor
    actor: user
    entities:
      - entity: stock-movement
        effect: reads
        facts:
          - Type
          - Quantity
          - Day
    contexts:
      desktop:
        place: desktop::profile::history
  - text: The Product removes the entry and recalculates the stock
    kind: product
    actor: user
    entities:
      - entity: stock-movement
        effect: removes
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::history
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/RetractFact.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/FactHistory.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/FactHistoryDialog.cs#FactHistoryDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Delete a mistaken stock movement

## Trigger

The person recorded a package or a correction by mistake.

## Outcome

The entry is gone and the stock and consumption are recalculated without it.
