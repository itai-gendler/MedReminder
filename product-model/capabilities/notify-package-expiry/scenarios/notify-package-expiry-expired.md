---
kind: alternate
routes:
  desktop: Desktop
steps:
  - text: The periodic check runs
    kind: condition
    entities: []
    contexts:
      desktop:
        place: desktop::profile
    unattended: true
  - text: A box the stock still covers passed its use-until date
    kind: condition
    entities:
      - entity: package
        effect: reads
        facts:
          - Use until
          - In stock
    contexts:
      desktop:
        place: desktop::profile
  - text: The Product gives the expired notice on the medicine's channels
    kind: product
    entities:
      - entity: medicine
        effect: reads
        facts:
          - Name
      - entity: package
        effect: reads
        facts:
          - Use until
    contexts:
      desktop:
        place: desktop::profile
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Packages/PackageExpiryNotices.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/PackageExpiryRules.cs#PackageExpiryRules
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Notify an expired box

## Trigger

The day after a box's use-until date.

## Outcome

One expired notice asks the person to check the box and mark it in MedReminder.

## Edge cases

- A box first seen already expired gets only the expired notice.
- Used-up and closed boxes give no notice.
