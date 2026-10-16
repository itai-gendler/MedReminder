# B.1 Android tablet, iOS and iPad implementation backlog

This backlog turns the post-M5 mobile scope into ordered, reviewable
implementation slices: Android tablets (track BT), the iOS app on iPhone
(track BI) and the iPad (track BP). It is a planning artifact for the
private mobile repository (`vger70/medreminder-mobile`) and this
repository's shared-core changes; it does not start implementation or
technical spikes.

It continues `ANALYSIS-B1-ANDROID-IMPLEMENTATION-BACKLOG.md`, whose
conventions it follows. It replaces the Phase 6 and Phase 7 entries of
`ANALYSIS-B1-MOBILE-SYNC.md` for iOS (the 15-25 d estimate there predates
the standalone-app requirement of `ANALYSIS-B1-ANDROID-PLAN.md`).

Tags: **[INFERRED]** for deductions, **[UNCERTAIN]** for claims not
verified against the tree, a device or the vendor documentation. Every
effort figure is [INFERRED]. Nothing in this document was run on iOS or
on a tablet.

## 1. Source of truth and delivery rules

- Product scope: `ANALYSIS-B1-ANDROID-PLAN.md` (A6 free apps, A7 private
  repository, DA9 no donation links). Screen behavior:
  `ANALYSIS-B1-UI-REQUIREMENTS.md`. Sync and platform constraints:
  `ANALYSIS-B1-MOBILE-SYNC.md` (§8 reminders, §9 storage, §16 decisions).
- The `ANALYSIS-B1-*` documents take precedence over `docs/STATUS.md`
  and `docs/EVOLUTION.md`.
- Tracks BT, BI and BP start after the Android public launch gate
  (DA4: M1-M5 as pre-release builds, public launch after M5), except the
  lead-time items of section 4, which start now. Vital tracking (D.1)
  stays evolution V and is not part of this backlog.
- Shared domain, application and portable changes go to this repository
  first, are merged and tagged `core-v*`, then the submodule reference
  of the private repository advances. UI and platform adapters stay in
  the private repository (A7).
- Every slice includes strings in all five languages (the mobile
  dictionaries and `MobileStringsTests` enforce it), screen-reader
  semantics, scalable text, loading/empty/error/offline states, and the
  verification listed in section 6. No later-track control appears
  active early.
- The required status check `Android build` of the private repository
  must keep passing. iOS CI starts as a non-required job.
- The apps stay entirely free: no slice adds a purchase, a premium gate
  or a donation link on any platform.
- Apple review rules for a medicine-related app are checked against the
  current App Review Guidelines at BI-12; the app keeps the desktop
  wording that it is not a medical device.

## 2. State of the code this backlog starts from

Read from `vger70/medreminder-mobile` on 2026-10-16:

- 45 pages in `src/MedReminder.Mobile/Pages/`, all in C#, no XAML.
- `Ui.Stack()` and `Ui.Scroll()` apply no maximum width; a wide window
  stretches every page edge to edge. `Ui.TwoColumns` already exists.
- Navigation is a four-tab `TabBar` in `AppShell.cs` (Today, Medicines,
  Agenda, More). Detail screens are pushed `ContentPage`s.
- `MainActivity` declares `ConfigurationChanges` for `ScreenSize`,
  `Orientation`, `ScreenLayout`, `SmallestScreenSize` and `Density`, so
  a resize does not recreate the activity; layouts must react to
  `SizeChanged`. The manifest locks no orientation and no resizability.
- All platform registration in `MauiProgram.cs` sits in one `#if ANDROID`
  block; `Platforms/` holds `Android` only (about 1,600 lines of
  adapters). `MedReminder.Mobile.Core` is `net10.0` with no MAUI and no
  Android API and already holds the logic that iOS can reuse.
- Android-only package references: `Xamarin.AndroidX.*`,
  `Xamarin.GooglePlayServices.Auth`. Cross-platform: SkiaSharp (PDF),
  ZXing.Net.Maui (QR and barcode), Microsoft.Identity.Client.
- `SyncEngineOptions.Platform` is the literal `"android"`.

## 3. Decisions and gates

| Key | Decision | Recommendation / status | Blocks |
|---|---|---|---|
| DT1 | Reverse the non-goal "tablet-specific layouts" (`ANALYSIS-B1-MOBILE-SYNC.md` §15) | Reverse for Android tablets and iPad: one adaptive layer, no separate tablet app. **Open.** | BT-01 |
| DT2 | Wide-screen navigation | Shell flyout locked open from the medium width class (side rail effect). A custom NavigationRail handler costs more and is not recommended [INFERRED]. **Open.** | BT-03 |
| DT3 | Adaptive layer lives in shared `Pages/`, not in `Platforms/Android/` | Recommended, so iPad reuses it. **Open.** | BT-01..04, BP-01 |
| DI1 | Apple Developer Program account type | Individual shows the personal name on the App Store and cannot invite collaborators; organization needs a D-U-N-S number and a legal entity. Same annual fee [UNCERTAIN: third-party sources, check developer.apple.com]. Mirrors DA5 (personal Play account); decide before enrolling because conversion later needs support contact. **Open.** | BI-00 |
| DI2 | iPad ships with the first iOS release | Recommended: universal binary from BI-02, iPad work is small once BT is done. **Open.** | BP-01 |
| DI3 | iOS minimum version | iOS 15 is an inferred proposal and unmeasured (`ANALYSIS-B1-MOBILE-SYNC.md` D13); AES-GCM needs iOS 13+. Take the floor of the MAUI 10 iOS workload and verify at BI-01 on the oldest device available; do not claim a floor before TestFlight. **Open.** | BI-01 |
| DI4 | Bundle identifier | Reuse `com.vger70.medreminder` (the identifier is independent of the Play application ID). **Open.** | BI-00 |
| DI5 | Apple-native iCloud transport | Stays excluded (D12). Cloud backup and sync use Google Drive and OneDrive on iOS too. **Decided 2026-10-08.** | - |
| DI6 | Donation links on iOS | **Decided** (DA9, D14): none. | - |
| DA3, A6, A7 | Language and catalogue defaults, free apps, private repository | Unchanged; apply to every track. | - |

## 4. Lead-time items (start now, no code)

| ID | Item | Done when |
|---|---|---|
| BI-00a | Apple Developer Program enrollment (DI1); with an organization account, obtain the D-U-N-S number first (free, can take days) | Account active, team ID known |
| BI-00b | Build host: Apple-silicon Mac on the macOS version required by the Xcode version that the .NET iOS workload pins. The workload accepts one Xcode version; a newer or older Xcode fails the build, so the pinned pair is written in the private repository's docs | `dotnet workload install maui-ios` and an empty MAUI iOS app run on a simulator |
| BI-00c | Test devices: one iPhone (oldest supported) and one iPad. Simulators do not validate notification delivery, biometrics or background tasks | Both enrolled in the provisioning profile |
| BI-00d | Identifiers: App ID with the capabilities of BI-04 to BI-08, certificates, provisioning profiles, App Store Connect API key for CI upload | Signed debug build installs on both devices |
| BI-00e | OAuth clients: Google iOS client ID with a custom URL scheme; Entra redirect `msauth.<bundle id>://auth` on the existing registration | Both registered and recorded (no secrets in the repository) |
| BT-00 | Android test hardware: one 8-inch and one 10-inch tablet, plus the Pixel Tablet and foldable emulators | Devices or images available to the testers |

## 5. Backlog by track

### BT - Android tablets

**Entry:** DT1-DT3 decided; the Android public launch gate passed, or the
tablet work runs on a branch behind it. **Exit:** every screen of M1-M5
is usable and reviewed on compact, medium and expanded widths, in both
orientations, in split-screen and in a resized window, with no data loss
on rotation or resize.

Android 16 (API 36) ignores `screenOrientation`, `resizeableActivity`
and aspect-ratio limits on screens of 600 dp and wider unless the app
opts out ([Android documentation](https://developer.android.com/develop/adaptive-apps/guides/app-orientation-aspect-ratio-resizability)).
Rotation, split-screen and free-form windows must therefore be handled
whether or not the app supports them. The app does not opt out.

| ID | Slice | Acceptance / done when | Depends on |
|---|---|---|---|
| BT-01 | `WindowSizeClass` and the adaptive helper | A pure `WindowSizeClass` in `MedReminder.Mobile.Core` (compact below 600 dp, medium 600-839, expanded 840 and above) with unit tests at the boundaries; a `Ui.Adaptive` helper in `Pages/` that raises on `SizeChanged`; no platform API in the Core type | DT1, DT3 |
| BT-02 | Content width cap and multi-column pages | Forms and lists are centered with a maximum width of about 600 dp; Today, Settings and More use two columns from the medium class (`Ui.TwoColumns`); text scaling still reflows without clipping | BT-01 |
| BT-03 | Wide navigation | Bottom tabs on compact; from medium, the locked Shell flyout lists the same four destinations (DT2); the selected destination survives rotation and resize; screen-reader order matches visual order | BT-01, DT2 |
| BT-04 | List-detail panes | Medicines, Agenda and History show list and detail side by side from the expanded class; the detail content is a reusable view (not only a pushed `ContentPage`), the compact layout keeps today's push navigation; selection and unsaved-change state survive rotation and resize; the unsaved-changes dialog (`FormUi`) still guards leaving a form | BT-01, BT-02 |
| BT-05 | Camera, pairing and sharing on tablets | QR pairing (the 260-wide code) and barcode scan work with a front-only camera or no camera (the permission is already optional); PDF and `.mrz` share/save work; app lock and reminders behave as on phones | BT-02 |
| BT-06 | Tablet verification matrix | Results recorded for: Pixel Tablet and foldable emulators, both orientations, split-screen, free-form window, font and display-size scaling, hardware keyboard focus order, the two real tablets; no overlap, clipping or lost input | BT-02..05, BT-00 |
| BT-07 | Play listing and guides | 7-inch and 10-inch screenshots; user guides in all five languages describe the wide layout; Play Console large-screen report reviewed [UNCERTAIN: Google's current large-screen quality criteria were not verified for this document] | BT-06 |

Effort: about 14-22 days [INFERRED]; BT-04 is the largest item.

### BI - iOS (iPhone)

**Entry:** BI-00a..e done; the Android public launch gate passed (DA4).
**Exit:** the manual checklist of the B.1 plan passes on a supported
iPhone from TestFlight, and App Store review passes.

| ID | Slice | Acceptance / done when | Depends on |
|---|---|---|---|
| BI-01 | iOS spikes | S3: EF Core and Microsoft.Data.Sqlite run on a device under iOS AOT with the Release defaults (iOS has no JIT, so reflection-based EF Core and `System.Text.Json` may need trimmer roots or source generation); S1: AES-GCM works; S5: local notifications fire on time, and the pending-notification limit is measured (documented as 64 per app [UNCERTAIN]); the oldest supported iOS version (DI3) is exercised. Results recorded in `ANALYSIS-B1-MOBILE-SYNC.md` §18 style | BI-00b, BI-00c |
| BI-02 | Multi-target skeleton | `net10.0-android;net10.0-ios` with the iOS target conditional on macOS so Linux and Windows builds are unchanged; the `#if ANDROID` registration of `MauiProgram.cs` moves into per-platform partial classes; Android-only package references become conditional on the target framework; `Platforms/iOS/` exists; `SyncEngineOptions.Platform` becomes `"ios"` on iOS (shared core checked, and changed in this repository first if it validates the value); `CheckSharedCoreBoundary` and the strip target still pass; the `Android build` check is unaffected | BI-01 |
| BI-03 | App data, backup exclusion and file protection | Databases and settings stay in the sandbox; the profile directories are excluded from iCloud/device backup and carry a file-protection class chosen with the lock-state trade-off for background sync stated; exact APIs confirmed in BI-01 [UNCERTAIN]. A restore from `.mrz` and an upgrade keep the data | BI-02 |
| BI-04 | Reminders | `UNUserNotificationCenter` adapter behind the existing `IWindowsNotificationService`/scheduler/settings ports: authorization request and denial handling, the planner's rolling window within the measured pending limit, the closing "open MedReminder to keep reminders active" notice, snooze and open-medicine actions that never record an intake silently, lock-screen detail hidden by default (B2-09); there is no exact-alarm permission and no boot event, so reminders are re-planned on launch, on foreground and in background refresh; the "exact alarm" and "battery" cards are hidden on iOS; a Time Sensitive level is used only if a capability is justified [UNCERTAIN] | BI-02, BI-01 (S5) |
| BI-05 | Documents, share and URLs | `UIDocumentPickerViewController` for `.mrz` export/import and calendar/PDF output; the share sheet; `Launcher` for mailto and regional-service links (`LSApplicationQueriesSchemes`); the regional services list uses the iOS app links already carried in the data | BI-02 |
| BI-06 | Security | `LocalAuthentication` app lock with passcode fallback (`NSFaceIDUsageDescription`); a Keychain-backed `ICredentialProtector`; no secret in logs; the Keychain item accessibility class stated and tested across reboot and restore | BI-02 |
| BI-07 | Cloud providers | Google Drive sign-in through an OAuth library that supports the iOS client and its URL scheme (availability of a maintained .NET binding [UNCERTAIN]; if none, an AppAuth-style flow with the system browser session); OneDrive through MSAL with the keychain group and `OpenUrl` handler; one cloud account at a time as on Android; backup and restore of the same encrypted archive | BI-02, BI-06, BI-00e |
| BI-08 | Background work | `BGTaskScheduler` for sync, cloud backup and catalogue refresh with the permitted identifiers in the Info.plist; sync on foreground and after local writes stays the primary path because iOS decides the timing [INFERRED]; the UI states the limit like the Android battery guidance (B2-10), without a settings route that does not exist | BI-02, BI-07 |
| BI-09 | In-process restart after join | iOS cannot restart the app; the installation join (`IAppRestarter`) tears down and rebuilds the service provider and the shell in process and lands on the joined profile; no stale singleton keeps the old profile | BI-02 |
| BI-10 | Camera features | QR pairing and barcode scan through ZXing.Net.Maui on iOS (`NSCameraUsageDescription`); denial and no-camera states; the camera is requested only when a scan page opens | BI-02 |
| BI-11 | Platform polish | System language and region preselection (DA3) from iOS settings; dark mode; Dynamic Type; VoiceOver labels and focus order; the shell's tab bar with the current iOS styling; the PDF (SkiaSharp) renders with the embedded fonts | BI-02 |
| BI-12 | Store readiness and TestFlight | `PrivacyInfo.xcprivacy` (required-reason APIs) and the App Store privacy answers matching the actual flows (nothing leaves the device unless the user enables a provider, as in `PRIVACY.md`); export-compliance answer for the use of AES-GCM and any national declaration [UNCERTAIN]; review notes explaining the non-medical-device positioning and giving a way to test without an account; a macOS CI job that builds, signs and uploads to TestFlight with the App Store Connect API key; internal then external TestFlight round with the same manual checklist as Android | BI-03..11 |
| BI-13 | Documentation | Five-language user guides gain the iOS differences (reminder timing, background limits, permissions, no battery card); release checklist and `docs/RELEASE.md` gain the Apple section; README and third-party notices updated | BI-12 |

Effort: about 25-37 days [INFERRED], excluding the lead-time items and
review waits.

### BP - iPad

**Entry:** BT exit passed and BI-02 done (DI2: the binary is universal
from the start). **Exit:** the BT exit criteria hold on a real iPad in
portrait, landscape, Split View and a resized window, and the
screenshots are accepted by App Store Connect.

| ID | Slice | Acceptance / done when | Depends on |
|---|---|---|---|
| BP-01 | Universal binary and orientations | Device family iPhone and iPad; all four orientations on iPad; the adaptive layer of BT-01..04 is used unchanged; no layout is iPhone-only | BT-04, BI-02 |
| BP-02 | Popover anchors and sheets | Share sheet, document picker and any menu present as popovers anchored to the tapped control on iPad (a missing anchor crashes there [UNCERTAIN]) | BI-05 |
| BP-03 | Multitasking and windows | Split View, Slide Over and resizable windows keep state through resize; check the current scene-lifecycle requirement of the Apple SDK the project must use and adopt it if required [UNCERTAIN]; the app does not claim full-screen-only | BP-01 |
| BP-04 | Keyboard and pointer | Hardware keyboard focus order and shortcuts for the main destinations; pointer hover states; text fields and pickers usable with the keyboard | BP-01 |
| BP-05 | iPad verification and store assets | Real-iPad run of the manual checklist; 13-inch iPad screenshots in the store listing [UNCERTAIN: current required sizes]; TestFlight round on iPad | BP-01..04, BI-12 |

Effort: about 4-7 days [INFERRED].

## 6. Cross-cutting definition of done

Section 4 of `ANALYSIS-B1-ANDROID-IMPLEMENTATION-BACKLOG.md` applies to
every slice. In addition:

1. No behavior difference between platforms in the shared core: stock,
   schedule and sync results come from the same use cases and are
   covered by the existing `MedReminder.Mobile.Core.Tests`; platform
   adapters are verified on devices.
2. A layout change is verified at each width class and orientation, with
   text scaling at the largest setting; rotation or resize never loses
   unsaved form input.
3. Each platform adapter states in the UI what the OS limits (reminder
   timing, background sync) instead of hiding it.
4. Logs contain no secrets or health data on any platform; no analytics
   or crash-reporting SDK is added.
5. Every new permission string, privacy label and store answer matches
   what the code does and is reviewed with the privacy policy
   (`PRIVACY.md` and its translations).
6. New UI strings exist in all five dictionaries; guides are updated in
   all five languages.

## 7. Recommended execution order

1. Now: decide DI1 and start BI-00a (enrollment and, if needed, the
   D-U-N-S number), BI-00b..e and BT-00. Decide DT1-DT3, DI2-DI4.
2. Run BI-01 as soon as the Mac and devices exist. A failed S3 or S5 on
   iOS changes the iOS estimate and is reported before BI-02.
3. Deliver BT-01..04 (the shared adaptive layer), then BT-05..07; ship
   the tablet build on the existing Play tracks.
4. Deliver BI-02..11 in the table order where dependencies allow
   (BI-04 reminders and BI-07 providers carry the most risk), then
   BI-12 and BI-13.
5. Deliver BP-01..05 against the first TestFlight builds, so the first
   App Store submission is universal (DI2).
6. Submit to the App Store after the TestFlight exit; release notes and
   the website state the supported devices.

This ordering refines the plan into work slices; it does not change the
approved M0-M5 scope.

## 8. Sources

- [Android: app orientation, aspect ratio and resizability](https://developer.android.com/develop/adaptive-apps/guides/app-orientation-aspect-ratio-resizability)
  (API 36 behavior on screens of 600 dp and wider).
- [dotnet/maui#34295](https://github.com/dotnet/maui/issues/34295) (the
  .NET iOS SDK requires one specific Xcode version).
- Apple Developer Program, account types and fee: third-party summaries
  only; confirm on developer.apple.com before enrolling.
