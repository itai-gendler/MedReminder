---
entities:
  - entity: medicine
    shows:
      - Name
      - Stock
      - Unit
  - entity: stock-movement
    collects:
      - Type
      - Quantity
      - Notes
  - entity: package
    collects:
      - Expiry
      - Use within
      - Opened on
      - Batch
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/StockAdjustmentDialog.cs#StockAdjustmentDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/PackageInputs.cs
---

# Stock movement window

One stock movement of the selected medicine, with the current stock: a new package (with its boxes, expiry and batch), a manual addition, or a positive or negative correction.
