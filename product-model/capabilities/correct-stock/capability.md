---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/AdjustStockDown.cs#AdjustStockDown
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/StockAdjustmentDialog.cs#StockAdjustmentDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Correct stock

Subtract units the person no longer has, such as a lost tablet or a spilled bottle.
