---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: The periodic check runs
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: The product code of an active medicine is on the shortage list and that shortage was not notified
    kind: condition
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Product code
      - entity: shortage
        effect: reads
        facts:
          - Product code
          - Start
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product notifies the shortage, with its start, expected end and reason, naming no substitute
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Notification channels
      - entity: shortage
        effect: reads
        facts:
          - Start
          - Expected end
          - Reason
    contexts:
      desktop:
        place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/ShortageNotices.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/ShortageTexts.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Notify a medicine in shortage

## Trigger

The downloaded AIFA list names the package of an active medicine.

## Outcome

One notification per shortage on the medicine's channels; the medicine shows In shortage or Shortage from a date in the Supply column.

## Edge cases

- When the package is also in the transparency list, the notice points to Equivalent medicines (AIFA).
- The person is advised to ask the doctor or pharmacist and request the prescription in good time.
