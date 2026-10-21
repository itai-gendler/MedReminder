---
entities:
  - entity: medicine
    shows:
      - Name
      - Schedule
      - Therapy start date
      - Therapy end date
      - Run-out date
      - Status
  - entity: suspension
    shows:
      - From
      - To
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/TherapyTimelineForm.cs#TherapyTimelineForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Timeline/TherapyTimelineBuilder.cs
---

# Therapy timeline

One row per medicine on a calendar from 60 days back to 120 days ahead: therapy in progress, suspensions, new doses and taper stages, and the estimated run-out date.
