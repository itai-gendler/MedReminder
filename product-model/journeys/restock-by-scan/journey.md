---
actors:
  - user
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/RestockByScanQuery.cs#RestockByScanQuery
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Restock by scanning the box

## Goal

The person records a purchased box without looking for the medicine in the list.

## Success criterion

The new package is recorded on the medicine the scanned code identified.
