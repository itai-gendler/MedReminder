---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Tick Therapy → Show inactive medicines
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product lists the inactive medicines beside the active ones
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Overview/MedicineListFilter.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Overview/MedicineOverviewLoader.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Calculations/IntradayConsumption.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Show inactive medicines

## Trigger

The person wants to see a medicine whose therapy was stopped.

## Outcome

Inactive medicines appear again in the list, in secondary text, with their history kept.
