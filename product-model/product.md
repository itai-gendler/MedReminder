---
id: medreminder
summary: >-
  A Windows desktop app that warns in time, before a medicine runs out, to ask the doctor for a new
  prescription; it also follows prescriptions, administrative deadlines and package expiry, for several people
  and on several PCs.
category: health
tags:
  - medicines
  - reminders
  - prescriptions
  - desktop
authors:
  - name: vger70
    url: https://github.com/vger70
license: Apache-2.0
languages:
  - en
  - it
  - fr
  - es
  - de
limitations:
  - >-
    MedReminder is an organizational reminder, not a medical device: it gives no diagnoses or therapy
    instructions and does not check doses, overdoses or drug interactions.
  - >-
    It does not track whether doses were taken and does not alert on missed doses; the dose-time reminder is
    only a prompt.
  - >-
    It never orders medicines and never contacts the doctor by itself; a prescription request leaves only when
    the person sends it.
  - Restoring a backup or importing an export always replaces a profile's data; nothing is merged.
  - >-
    Sync and the shared installation work through storage every device reaches (OneDrive, Google Drive or a
    shared folder), and every device of a group runs the same MedReminder version; MedReminder runs no server.
references:
  - kind: doc
    role: intent
    target: README.md
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
  - kind: doc
    role: context
    target: docs/ANALYSIS.md
  - kind: doc
    role: context
    target: PRIVACY.md
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Program.cs
---

# MedReminder

MedReminder is a Windows desktop app that follows the stock of the medicines a person takes and warns, with a lead time they choose, when it is time to ask the doctor for a new prescription. It deducts the scheduled doses every day, from simple daily doses to weekly patterns, cycles and tapers, and estimates the run-out date. Around that core it follows prescriptions to the pharmacy, administrative deadlines, package expiry and Italian shortage and equivalence lists, reminds at dose time, and sends Windows notifications and e-mails, also to a caregiver. Several people share one installation through profiles with administrator and user roles, and a profile or the whole installation can be kept on several PCs.

## Intent

> Windows desktop app that reminds the user to request a new prescription from their doctor in time, before a medicine's stock runs out.

> Keep track of the stock of medicines you take regularly and warn you, with a configurable lead time, when it is time to ask your doctor for a new prescription. The app computes days left and estimated run-out date from the therapy schedule, suspensions and schedule changes; it sends a Windows notification and/or an email when the stock falls below the threshold. Optionally, it also reminds the user at each scheduled dose time.

> MedReminder is an organizational reminder, not a medical device. It does not provide diagnoses, therapy instructions, therapy changes or clinical suggestions. Every therapy decision must be made with your doctor.
