---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: Click Regional prescription service, then Open on phone
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Region
      - entity: prescription
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: The Product shows the QR code of the region's app or portal
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Region
    contexts:
      desktop:
        place: desktop::profile::prescriptions
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/RegionalServicePanel.cs#RegionalServicePanel
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Prescriptions/RegionalServiceForProfileQuery.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Prescriptions/RegionalHealthService.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileSettingsUseCases.cs#UpdateProfileRegion
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Open the regional app on the phone

## Trigger

The person prefers to sign in on the phone.

## Outcome

A QR code of the regional app, or of the portal when there is no app, is shown to scan with the phone camera.
