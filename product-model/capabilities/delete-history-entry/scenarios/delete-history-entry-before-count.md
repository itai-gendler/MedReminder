---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Select an entry recorded before the latest stock count
    kind: actor
    actor: user
    entities:
      - entity: stock-movement
        effect: reads
        facts:
          - Type
          - Day
      - entity: stock-count
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::history
  - text: >-
      A later stock count already includes the entry, or it was recorded before the update that made entries
      deletable
    kind: condition
    entities:
      - entity: stock-count
        effect: reads
        facts:
          - Day
    contexts:
      desktop:
        place: desktop::profile::history
  - text: The Product shows why the entry cannot be deleted and keeps Delete unavailable
    kind: product
    actor: user
    entities: []
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

# Refuse to delete an entry before the latest count

## Trigger

The person selects an entry recorded before the latest stock count, or before the history could be deleted.

## Outcome

Delete stays unavailable; an older mistake is fixed by counting the stock again.
