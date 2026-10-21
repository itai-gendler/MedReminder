---
appliesTo:
  - type: capability
    id: notify-shortage
  - type: entity
    id: shortage
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/ShortageTexts.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A shortage notice names no substitute

The notice gives the start, expected end and reason as AIFA lists them and advises asking the doctor or pharmacist.
