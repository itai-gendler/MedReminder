---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Select the medicine and choose Therapy → Delete…
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
  - text: No stock movement, intake, count or suspension was recorded for it, not even an initial stock
    kind: condition
    entities:
      - entity: stock-movement
        effect: reads
        facts: []
      - entity: intake
        effect: reads
        facts: []
      - entity: suspension
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Confirm
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product removes the medicine with its schedule and administration times
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: removes
        from: Active
    contexts:
      desktop:
        place: desktop::profile::medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/DeleteMedicine.cs#DeleteMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Delete a medicine entered by mistake

## Trigger

The person added a medicine by mistake.

## Outcome

The medicine is gone; with sync on, it disappears from the other devices too.
