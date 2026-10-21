---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Right-click the medicine and choose Equivalent medicines (AIFA)…
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product finds the group by the medicine's product code only
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Product code
      - entity: equivalence-group
        effect: reads
        facts:
          - Group
    contexts:
      desktop:
        place: desktop::profile::equivalent-medicines
  - text: >-
      The Product lists every pack of the group, cheapest first, with its price, the difference over the
      reference price, its supply state, any medicine of the profile that already holds it, and the AIFA note
      verbatim
    kind: product
    actor: user
    entities:
      - entity: equivalence-group
        effect: reads
        facts:
          - Group
          - Reference price
          - Packages
          - Note
      - entity: shortage
        effect: reads
        facts:
          - Start
          - Expected end
      - entity: profile
        effect: reads
        facts: []
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::equivalent-medicines
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

# Compare the equivalents of a package

## Trigger

A medicine is in shortage, or the person wants to know what the pharmacy may offer instead.

## Outcome

The group, the reference price and every package of the group are shown, the person's own in bold; substitution stays with the doctor and pharmacist.

## Edge cases

- A package missing from the list is not "without equivalents": it may be patented, in class C or simply not listed.
- Class C medicines have no public price list and show no price.
