---
kind: primary
routes:
  desktop: Desktop
steps:
  - text: Click Regional prescription service
    kind: actor
    actor: user
    entities:
      - entity: prescription
        effect: reads
        facts: []
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: The profile has no region yet
    kind: condition
    entities:
      - entity: profile
        effect: reads
        facts:
          - Region
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: Choose the region or autonomous province
    kind: actor
    actor: user
    entities:
      - entity: profile
        effect: reads
        facts:
          - Region
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: >-
      The Product saves the region with the profile and names the service and how to sign in (SPID, CIE or
      TS-CNS)
    kind: product
    actor: user
    entities:
      - entity: profile
        effect: changes
        facts:
          - Region
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: Click Open in browser
    kind: actor
    actor: user
    entities: []
    contexts:
      desktop:
        place: desktop::profile::prescriptions
  - text: The Product opens the regional portal in the person's browser
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

# Choose the region and open its service

## Trigger

The person wants to copy a prescription number instead of waiting for it.

## Outcome

The regional portal opens in the browser; the region is saved with the profile.

## Edge cases

- MedReminder does not see the credentials or the health record and imports nothing from it.
- When no service is listed for the region, the person opens their region's health record portal themselves.
