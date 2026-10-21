---
appliesTo:
  - type: capability
    id: delete-history-entry
  - type: capability
    id: count-stock
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/FactHistory.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Only entries after the latest stock count can be deleted

A stock count already includes the mistakes before it, so they are fixed by counting again; entries recorded before the history could be deleted are fixed with a correction.
