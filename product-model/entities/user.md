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
    target: src/MedReminder.Application/Abstractions/Profile.cs#Profile
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# User

A person whose profile has the user role: they keep their own medicines, stock, prescriptions, deadlines and recipients, and see only their own profile.
