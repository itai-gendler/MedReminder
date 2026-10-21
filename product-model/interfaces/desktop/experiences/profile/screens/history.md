---
entities:
  - entity: medicine
    shows:
      - Name
  - entity: stock-movement
    shows:
      - Type
      - Quantity
      - Day
      - Notes
  - entity: intake
    shows:
      - Day
      - Quantity
      - Status
      - Notes
  - entity: stock-count
    shows:
      - Day
      - Counted quantity
      - Correction
      - Notes
  - entity: suspension
    shows:
      - From
      - To
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/FactHistoryDialog.cs#FactHistoryDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Ledger/FactHistory.cs
---

# History

The stock movements, intakes, counts and suspensions recorded for one medicine, newest first, each with whether it can still be deleted.
