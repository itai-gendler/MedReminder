---
entities:
  - entity: medicine
    shows:
      - Name
      - Active ingredient
      - Package
      - Unit
      - Schedule
      - Administration times
      - Therapy start date
      - Therapy end date
      - Warning threshold
      - Notification channels
      - Remind at dose time
      - Reference doctor
      - Notes
      - Product code
    collects:
      - Name
      - Active ingredient
      - Package
      - Unit
      - Schedule
      - Administration times
      - Therapy start date
      - Therapy end date
      - Warning threshold
      - Notification channels
      - Remind at dose time
      - Reference doctor
      - Notes
  - entity: catalogue-medicine
    shows:
      - Name
      - Active ingredient
      - Package
      - Product code
      - Marketing state
      - Information links
  - entity: stock-movement
    collects:
      - Quantity
  - entity: moment
    shows:
      - Name
      - Time
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MedicineEditDialog.cs#MedicineEditDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/AdministrationSlotDialog.cs#AdministrationSlotDialog
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Controls/SchedulePanel.cs#SchedulePanel
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Medicine window

One medicine being added or edited: its name and package (suggested by the catalogue or read from a barcode), unit, schedule, administration times, start and end dates, warning threshold, channels, dose-time reminder, reference doctor and notes. For a medicine with an Italian product code, its information links.
