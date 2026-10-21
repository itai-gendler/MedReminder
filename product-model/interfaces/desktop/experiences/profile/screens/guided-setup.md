---
entities:
  - entity: profile
    shows:
      - Name
      - Recipient
      - Caregiver e-mail
      - Copy to the caregiver
      - Weekly stock summary
    collects:
      - Name
      - Recipient
      - Caregiver e-mail
      - Copy to the caregiver
      - Weekly stock summary
  - entity: medicine
    shows:
      - Name
      - Days left
  - entity: preferences
    collects:
      - New medicine warning
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/GuidedSetupForm.cs#GuidedSetupForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/GuidedSetup/GuidedSetupFlow.cs
---

# Guided setup

A short wizard from an empty list to medicines that warn: who the medicines are for, the first medicines, when and how to warn, the e-mail addresses and a summary.
