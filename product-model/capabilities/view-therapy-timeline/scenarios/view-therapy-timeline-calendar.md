---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Therapy → Therapy timeline…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: >-
      The Product draws each medicine's therapy, suspensions, dose and regime changes, taper stages and
      run-out date
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Schedule
          - Therapy start date
          - Therapy end date
          - Run-out date
          - Status
      - entity: suspension
        effect: reads
        facts:
          - From
          - To
    contexts:
      desktop:
        place: desktop::profile::therapy-timeline
  - text: Move Earlier or Later by 30 days and select a medicine
    kind: actor
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
    contexts:
      desktop:
        place: desktop::profile::therapy-timeline
  - text: The Product describes the selected medicine in words; Show in list selects it in the main list
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Schedule
          - Run-out date
    contexts:
      desktop:
        place: desktop::profile::therapy-timeline
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Timeline/TherapyTimelineBuilder.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/TherapyTimelineForm.cs#TherapyTimelineForm
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Look at the coming months

## Trigger

The person wants an overview of the therapies and when each medicine runs out.

## Outcome

Each medicine is a row from 60 days back to 120 days ahead, and the Details box describes the selected one in words.

## Edge cases

- A deactivated medicine is a grey row.
- While a therapy is suspended, no run-out date is shown.
