---
entities:
  - entity: medicine
    shows:
      - Name
      - Stock
      - Schedule
      - Unit
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/CoveragePlannerDialog.cs#CoveragePlannerDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Coverage/CoveragePlanner.cs#CoveragePlanner
---

# Supply plan

For a period the person chooses, each active medicine with the quantity the period needs, the stock at its start, what is missing and how many packages to get.
