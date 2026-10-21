---
availability:
  - place: desktop::profile
domain: medicines
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

# Delete a history entry

Undo a mistaken entry of a medicine (a stock movement, an intake, a count or a suspension) from its history; stock and consumption are recalculated.
