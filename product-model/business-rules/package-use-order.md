---
appliesTo:
  - type: entity
    id: package
    facts:
      - In stock
  - type: capability
    id: view-expiring-packages
  - type: capability
    id: notify-package-expiry
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/PackageAllocation.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# The open box is used first, then the ones that expire first

The stock is spread over the boxes in that order; a box the stock no longer covers is Used up and gives no notice, even if never marked finished.
