---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Type part of the name in the search box
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
  - text: The Product filters the list by name as the person types
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Stock
          - Days left
          - Run-out date
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

# Find a medicine by name

## Trigger

The person looks for one medicine in a long list.

## Outcome

The list shows only the medicines whose name matches; Esc in the search box shows all of them again.
