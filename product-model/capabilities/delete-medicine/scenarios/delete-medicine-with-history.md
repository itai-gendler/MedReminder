---
kind: validation
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
  - text: A stock movement, intake, count or suspension is recorded for it
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
  - text: The Product refuses and suggests deactivating it or retracting the entries in Stock → History
    kind: product
    actor: user
    entities: []
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

# Refuse to delete a medicine with history

## Trigger

The person tries to delete a medicine that already has entries.

## Outcome

Nothing is deleted; the person is told to deactivate the medicine, or first delete the recorded entries from the history.

## Edge cases

- An entry recorded meanwhile, by a sync or a reminder, wins over a confirmation already given.
