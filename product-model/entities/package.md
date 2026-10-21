---
domain: packages
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/StockPackage.cs#StockPackage
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/PackageExpiryRules.cs#PackageExpiryRules
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Stock/PackageAllocation.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Package

One physical box of a medicine in the cabinet, with its printed expiry and its period of use after opening. Packages are an inventory beside the stock: recording, opening, finishing or deleting a package never changes the stock; only discarding one removes what was left in it.

## Information kept

- **Quantity** — the units in the box when full
- **Expiry** — the printed month and year; the box is good until the last day of that month
- **Use within** — days of use after opening, as the leaflet says
- **Opened on** — the first opening
- **Batch** — optional, as printed or scanned
- **Use until** — the earlier of the printed expiry and the last day of use after opening
- **In stock** — how much of the stock the box still holds: the open box is used first, then the ones that expire first
- **Status** — Valid, Expires soon, Expired, Used up or Closed
- **Closed on** — the day the box left the cabinet

## States

### Unopened

The box is in the cabinet and has not been opened.

### Opened

The box is in use; its period after opening runs.

### Finished

The box was used up and left the cabinet.

### Discarded

The box was thrown away, typically expired; what was left in it left the stock.
