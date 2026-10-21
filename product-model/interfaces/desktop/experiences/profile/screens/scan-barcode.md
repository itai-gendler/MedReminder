---
entities:
  - entity: catalogue-medicine
    shows:
      - Name
      - Product code
  - entity: medicine
    shows:
      - Name
      - Product code
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/BarcodeScanDialog.cs#BarcodeScanDialog
  - kind: code
    role: implementation
    target: src/MedReminder.Application/Catalogue/BarcodeParser.cs#BarcodeParser
---

# Scan barcode

Reads the code of a box with a USB scanner, a typed code or the webcam, and looks it up in the catalogue.
