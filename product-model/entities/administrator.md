---
kind: person
acts: external
relations:
  - entity: profile
    verb: uses
    cardinality: one-to-one
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/ProfileAdministrationUseCases.cs#ProfileAdministration
  - kind: code
    role: implementation
    target: src/MedReminder.Infrastructure.Portable/Profiles/CurrentProfile.cs#CurrentProfile
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Administrator

A person whose profile has the administrator role. Besides their own medicines, they look after what the whole installation shares: the e-mail account, backup, the reference country, the other profiles, sync between PCs and the shared installation. The first profile created on a PC is an administrator, and at least one always remains.
