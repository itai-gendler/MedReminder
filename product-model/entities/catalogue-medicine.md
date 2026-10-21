---
relations:
  - entity: medicine
    verb: describes
    cardinality: one-to-many
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Catalogue/ReferenceMedicine.cs#ReferenceMedicine
  - kind: doc
    role: intent
    target: docs/CATALOGUE-DATA.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Catalogue medicine

A package in the reference catalogue of the reference country: Italy (AIFA), Spain (AEMPS), France (ANSM), the medicines authorised for the whole European Union (EMA), or the United States (FDA). Picking one fills in a medicine.

## Information kept

- **Name** — commercial name, with strength and form when known
- **Active ingredient** — one or more
- **Package** — the package description
- **Product code** — the national code (AIC, EMA product number, NDC…)
- **Marketing state** — whether the product is suspended or withdrawn
- **Information links** — the package leaflet and the summary of product characteristics, where the source has them
