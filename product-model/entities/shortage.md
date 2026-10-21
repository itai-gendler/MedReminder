---
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Catalogue/ShortageList.cs#ShortageEntry
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/ShortageFeed.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Shortage

A package listed by AIFA as hard to find, or announced as in shortage from a date. MedReminder downloads the list with the Italian catalogue and names no substitute.

## Information kept

- **Product code** — the AIC of the package listed
- **Start** — when the shortage started or starts
- **Expected end** — often not communicated, and it may change
- **Reason** — as AIFA gives it
- **Equivalents reported** — whether AIFA reports equivalent medicines
- **List date** — the date of the list
