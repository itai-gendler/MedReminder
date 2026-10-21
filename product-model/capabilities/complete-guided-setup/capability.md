---
availability:
  - place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/GuidedSetupForm.cs#GuidedSetupForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/GuidedSetup/GuidedSetupFlow.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/GuidedSetup/GuidedSetupUseCases.cs#ApplyGuidedSetupWarning
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Complete the guided setup

Go from an empty list to medicines that warn: say who the medicines are for, add the first medicines, choose when and how to warn, and give the e-mail addresses. The guided setup records only what the person types: it suggests no medicine and no dose.
