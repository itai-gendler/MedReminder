---
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Catalogue/EquivalenceList.cs#EquivalenceGroup
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/EquivalentsQuery.cs#EquivalentsView
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Equivalence group

A group of the AIFA transparency list: packages with the same active ingredient, units, strength and route, with the reference price of the national health service.

## Information kept

- **Group** — active ingredient, units, strength and route
- **Reference price** — the price the national health service refunds
- **Packages** — every package of the group, cheapest first, with its public price and the difference over the reference price
- **Note** — the AIFA note, verbatim; it can restrict substitution
