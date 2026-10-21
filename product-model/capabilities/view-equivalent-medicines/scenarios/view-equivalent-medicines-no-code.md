---
kind: validation
routes:
  desktop: Desktop
steps:
  - text: Right-click a medicine entered by name only
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
  - text: The medicine has no Italian product code
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Product code
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product greys out Equivalent medicines (AIFA)… and Open Codifa page
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
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

# No equivalents for a medicine without an Italian code

## Trigger

The person looks for the equivalents of a medicine entered by name only.

## Outcome

The command is greyed out.
