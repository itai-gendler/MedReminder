---
type: desktop-app
actors:
  - user
  - administrator
languages:
  - en
  - it
  - fr
  - es
  - de
references:
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Program.cs
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Forms/MainForm.cs#MainForm
  - kind: code
    role: implementation
    target: src/MedReminder.UI/Tray/ApplicationTrayIcon.cs
  - kind: doc
    role: intent
    target: docs/USER_GUIDE.en.md
---

# MedReminder for Windows

The Windows desktop app: the main window with its menus and navigation, the windows it opens, the icon in the notification area, and the Windows notifications it shows. It keeps running in the notification area when its window is closed, so reminders still arrive. One profile is open at a time.
