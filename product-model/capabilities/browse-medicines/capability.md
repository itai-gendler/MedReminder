---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Overview/MedicineListFilter.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Overview/MedicineOverviewLoader.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/IntradayConsumption.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Browse medicines

See the open profile's medicines with their stock, days left, run-out date and status, narrowed to the empty, running-low or suspended ones, or by name.
