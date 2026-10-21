---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Select the suspension, click Delete and confirm
    kind: actor
    actor: user
    entities:
      - entity: suspension
        effect: reads
        facts:
          - From
          - To
    contexts:
      desktop:
        place: desktop::profile::history
  - text: The Product removes the suspension and recalculates the stock
    kind: product
    actor: user
    entities:
      - entity: suspension
        effect: removes
        from: Ongoing
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
          - Status
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

# Delete a suspension

## Trigger

A suspension recorded on another device was a mistake.

## Outcome

The days it covered are consumed again and the run-out date is estimated again.
