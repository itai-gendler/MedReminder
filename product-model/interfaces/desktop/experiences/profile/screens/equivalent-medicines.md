---
entities:
  - entity: equivalence-group
    shows:
      - Group
      - Reference price
      - Packages
      - Note
  - entity: medicine
    shows:
      - Name
      - Product code
  - entity: shortage
    shows:
      - Start
      - Expected end
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/EquivalentsDialog.cs#EquivalentsDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/EquivalentsQuery.cs#EquivalentsQuery
---

# Equivalent medicines

The AIFA transparency group of one package: the group, the reference price and every package of the group, cheapest first, with its supply state and whether the profile already has it at home.
