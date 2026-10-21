---
appliesTo:
  - type: entity
    id: medicine
    facts:
      - Remind at dose time
  - type: capability
    id: add-medicine
  - type: capability
    id: edit-medicine
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/Medicine.cs#Medicine
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A dose reminder needs a timed slot and stock

Remind at dose time can be turned on only for a medicine with at least one slot at a time of day and some stock; otherwise it is saved off.
