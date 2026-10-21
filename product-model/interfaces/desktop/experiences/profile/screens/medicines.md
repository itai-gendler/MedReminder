---
entities:
  - entity: medicine
    shows:
      - Name
      - Stock
      - Days left
      - Run-out date
      - Status
      - Supply
      - Expiry
      - Unit
  - entity: shortage
    shows:
      - Start
      - Expected end
      - Reason
      - Equivalents reported
      - List date
  - entity: profile
    shows:
      - Name
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Overview/MedicineOverviewLoader.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Medicine list

The list of the open profile's medicines, the main page of MedReminder. Above it, how many medicines are empty, running low or suspended; each row shows the stock, the daily rate, the days left, the run-out date, the status, the supply and the first expiry.
