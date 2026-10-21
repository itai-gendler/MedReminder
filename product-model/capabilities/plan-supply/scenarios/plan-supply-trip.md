---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Choose Therapy → Plan supply…
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::medicines
  - text: Choose From and To (the next 14 days by default)
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::plan-supply
  - text: >-
      The Product follows each schedule, suspension, end date and administration time through the period and
      counts what to get in units of the last purchase recorded
    kind: product
    actor: user
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
          - Stock
          - Schedule
          - Unit
      - entity: suspension
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::plan-supply
  - text: Print, save as PDF or copy the plan
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::plan-supply
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Coverage/CoveragePlanner.cs#CoveragePlanner
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/CoveragePlannerDialog.cs#CoveragePlannerDialog
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Check the supply for a trip

## Trigger

The person leaves for two weeks.

## Outcome

Each active medicine shows what the period needs, the stock when it starts, what is missing or covered, and how many packages to get; the medicines not covered come first.

## Edge cases

- As-needed medicines are shown but not computed.
- Runs out before is shown when nothing will be left at the start of the period.
- The window changes nothing: the figures are estimates.
