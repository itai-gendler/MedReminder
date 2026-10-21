---
appliesTo:
  - type: entity
    id: package
    facts:
      - Use until
  - type: capability
    id: notify-package-expiry
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/PackageExpiryRules.cs#PackageExpiryRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# A box expires at the end of its printed month, or earlier once opened

The use-until date is the earlier of the last day of the printed month and the last day of use after opening, the opening day counting as day 1.
