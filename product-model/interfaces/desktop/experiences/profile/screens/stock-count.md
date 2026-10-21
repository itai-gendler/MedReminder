---
entities:
  - entity: medicine
    shows:
      - Name
      - Stock
      - Run-out date
  - entity: stock-count
    collects:
      - Counted quantity
      - Already taken today
      - Notes
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/StockCountDialog.cs#StockCountDialog
---

# Stock count window

What the person counted, beside the expected stock, the difference and how the run-out date changes.
