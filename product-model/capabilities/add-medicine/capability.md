---
availability:
  - place: desktop::profile
domain: medicines
references:
  - kind: code
    role: implementation
    target: src/MedReminder.Application/UseCases/AddMedicine.cs#AddMedicine
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MedicineEditDialog.cs#MedicineEditDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/SearchCatalogueUseCase.cs
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/CatalogueEntryFill.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# Add a medicine

Record a medicine with its therapy, its initial stock and how it warns. From then on MedReminder deducts the daily dose by itself.
