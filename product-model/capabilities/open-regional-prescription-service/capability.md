---
availability:
  - place: desktop::profile
domain: prescriptions
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

# Open the regional prescription service

With Italy as reference country, open the service of the person's region where the electronic prescriptions issued to them are shown, in the browser or on the phone. The person signs in there, never in MedReminder.
