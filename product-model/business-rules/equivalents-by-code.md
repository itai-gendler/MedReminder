---
appliesTo:
  - type: capability
    id: view-equivalent-medicines
  - type: entity
    id: equivalence-group
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/EquivalentsQuery.cs#EquivalentsQuery
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Equivalent medicines are matched by product code only

The transparency list is joined with a medicine through its Italian product code, never by name or active ingredient, since a wrong match on strength or form would be unsafe. Substitution is decided by the doctor and the pharmacist.
