---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/EquivalentsQuery.cs#EquivalentsQuery
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/EquivalentsDialog.cs#EquivalentsDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# View equivalent medicines

For a medicine with an Italian product code in the AIFA transparency list, see the packages of its group, cheapest first, with prices, supply state and whether the profile already has one at home.
