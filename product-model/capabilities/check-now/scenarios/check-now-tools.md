---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Tools → Check now, or Check now from the notification-area icon
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: >-
      The Product deducts the scheduled doses of the days not yet deducted and checks every active medicine
      against its warning threshold
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: changes
        facts:
          - Stock
          - Days left
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: The Product reports how many medicines it checked and how many notifications it sent
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Status
    contexts:
      desktop:
        place: desktop::profile::medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/MedicationMonitor.cs#MedicationMonitor
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Tray/ApplicationTrayIcon.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Monitoring/ConsumptionCatchUp.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Check the medicines on demand

## Trigger

The person just changed the stock or a threshold and wants to know at once.

## Outcome

The status bar reports how many medicines were checked and how many notifications were sent.
