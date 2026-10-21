---
appliesTo:
  - type: capability
    id: warn-low-stock
  - type: capability
    id: add-package
  - type: capability
    id: count-stock
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/NotificationCycle.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Ledger/LedgerDeriver.cs#LedgerDeriver
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A medicine warns at its threshold and again at half of it, once per refill

The first warning comes when the days left reach the warning threshold, the second when they reach half of it with no stock added. Adding stock (a new package, a manual addition or a positive correction) starts the cycle again; so does a count that leaves more days than the threshold. No warning comes when the therapy ends before the run-out date.

## Rationale

Without the second stage, a person who missed the first warning heard nothing more until the medicine ran out.
