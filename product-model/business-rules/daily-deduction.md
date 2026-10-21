---
appliesTo:
  - type: entity
    id: medicine
    facts:
      - Stock
  - type: capability
    id: check-now
  - type: capability
    id: add-medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/ConsumptionMaterializer.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/ConsumptionCatchUp.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Scheduled doses are deducted every day by themselves

Each day the schedule in force for that day is deducted, so the person records only what differs. Doses taken as needed are never deducted by themselves, and days a medicine is inactive or suspended are not deducted.
