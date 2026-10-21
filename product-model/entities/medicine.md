---
domain: medicines
relations:
  - entity: stock-movement
    verb: has
    cardinality: one-to-many
  - entity: intake
    verb: has
    cardinality: one-to-many
  - entity: stock-count
    verb: has
    cardinality: one-to-many
  - entity: suspension
    verb: has
    cardinality: one-to-many
  - entity: package
    verb: has
    cardinality: one-to-many
  - entity: prescription
    verb: has
    cardinality: one-to-many
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/Medicine.cs#Medicine
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/Schedule.cs#Schedule
  - kind: code
    role: implementation
    target: src/MedReminder.Domain/Medicines/MedicationAdministrationSlot.cs#MedicationAdministrationSlot
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Overview/MedicineListItem.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Medicine

A medicine a person takes regularly or as needed, with its therapy, its stock and the warnings it gives. MedReminder deducts the scheduled doses every day and estimates when the stock runs out.

## Information kept

- **Name** — the name typed or picked from the catalogue
- **Active ingredient** — optional
- **Package** — optional description of the box
- **Product code** — the national code of the package when picked from the catalogue or scanned (the AIC in Italy); it links the medicine to the catalogue, the shortage list and the equivalent medicines
- **Unit** — tablets, capsules, ml or any unit typed
- **Schedule** — Simple (dose per administration × administrations per day) or an Advanced regime: weekly pattern, cyclic (N on / M off), linear or stepped tapering, or as needed (PRN); each version applies from its Effective from date
- **Administration times** — optional slots, each with a dose, an optional time or moment of the day, a label, and whether it is taken as needed
- **Therapy start date** — the first day of the therapy
- **Therapy end date** — optional last day of the therapy
- **Warning threshold** — how many days before running out the person wants to be warned
- **Notification channels** — Windows notification, Email, both or none
- **Remind at dose time** — whether a reminder comes at each timed slot
- **Reference doctor** — optional; the greeting of a prescription request uses the name
- **Notes** — optional free text, left out of the therapy report unless asked
- **Stock** — the quantity recorded: what the stock movements add and remove, less the doses deducted day by day
- **Days left** — how many days the stock lasts at the current schedule
- **Run-out date** — the estimated day the stock runs out
- **Status** — OK, Warning (running low), Empty, Suspended or Inactive, as the list shows it
- **Supply** — In shortage or Shortage from a date, when the package is on the AIFA shortage list
- **Expiry** — the first expiry among the packages in stock, marked expired or expires soon

## States

### Active

The therapy is followed: doses are deducted and warnings and reminders come.

### Inactive

The therapy was stopped: the medicine is hidden from the list, gets no reminders and is not consumed; its history is kept.
