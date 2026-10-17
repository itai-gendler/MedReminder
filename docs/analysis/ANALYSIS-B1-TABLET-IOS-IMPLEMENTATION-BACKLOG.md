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
on a tablet. Section 9 records how each step of section 5 was checked
against the code and the vendor sources on 2026-10-17.

## 1. Source of truth and delivery rules

- Product scope: `ANALYSIS-B1-ANDROID-PLAN.md` (A6 free apps, A7 private
  repository, DA9 no donation links). Screen behavior:
  `ANALYSIS-B1-UI-REQUIREMENTS.md`. Sync and platform constraints:
  `ANALYSIS-B1-MOBILE-SYNC.md` (§8 reminders, §9 storage, §16 decisions).
- The `ANALYSIS-B1-*` documents take precedence over `docs/STATUS.md`
  and `docs/EVOLUTION.md`.
- Tracks BT, BI and BP, including the lead-time items of section 4,
  start after the Android public launch gate (DA4: M1-M5 as pre-release
  builds, public launch after M5; decided 2026-10-16). Vital tracking (D.1)
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
- `SyncEngineOptions.Platform` is the literal `"android"`, set in two
  places: the default in `MobileServices.cs` and the registration in
  `MauiProgram.cs`. The shared core keeps it as a free string
  (`SyncEngine.cs`, default `"desktop"`); `DeviceItem.IsComputer`
  tests only for `"desktop"`.
- Notification permission calls use `Permissions.PostNotifications`
  directly in `Pages/Onboarding.cs` and `Pages/HouseholdPages.cs`;
  camera permission is requested only when a scan page opens
  (`HouseholdPages.cs`, `DevicePages.cs`, `BarcodePages.cs`).
- Sharing uses MAUI `Share.Default.RequestAsync` (calendar file,
  prescription request text) and `Launcher.TryOpenAsync`; `.mrz`
  export/import goes through the platform `IDocumentPicker`.
- `FormPage` guards unsaved changes in `OnBackButtonPressed` and
  `FormUi.Confirm`.
- The regional-service view exposes only the Android app link
  (`RegionalServiceView.AppUrl` returns `AndroidAppUrl`), although the
  shared domain and the feed also carry `IosAppUrl`.
- The app does not pin the .NET SDK (`global.json` absent) or
  `MauiVersion`; it targets `net10.0-android`.

## 3. Decisions and gates

| Key | Decision | Recommendation / status | Blocks |
|---|---|---|---|
| DT1 | Reverse the non-goal "tablet-specific layouts" (`ANALYSIS-B1-MOBILE-SYNC.md` §15) | **Decided 2026-10-16**: reversed for Android tablets and iPad; one adaptive layer, no separate tablet app. | BT-01 |
| DT2 | Wide-screen navigation | **Decided 2026-10-16**: Shell flyout locked open from the medium width class (side rail effect); no custom NavigationRail handler. | BT-03 |
| DT3 | Adaptive layer lives in shared `Pages/`, not in `Platforms/Android/` | **Decided 2026-10-16**, so iPad reuses it. | BT-01..04, BP-01 |
| DI1 | Apple Developer Program account type | **Decided 2026-10-16**: individual. The App Store shows the personal name and the account cannot invite collaborators; mirrors DA5 (personal Play account). Conversion to an organization later needs Apple support and a D-U-N-S number [UNCERTAIN: third-party sources, check developer.apple.com]. Trader status (EU DSA), **decided 2026-10-17**: the app is free (A6), with no purchase, subscription, advertising or donation link (DA9), so the intended declaration is non-trader, which publishes no contact details. Apple publishes a trader's address, phone number and email on the EU product pages ([Apple help](https://developer.apple.com/help/app-store-connect/manage-compliance-information/manage-european-union-digital-services-act-compliance-information)). The declaration is the developer's own assessment; revisit it if the app ever gains revenue or advertising [UNCERTAIN: the criteria are Apple's and the DSA's, not checked by a lawyer]. | BI-00a, BI-00f |
| DI2 | iPad ships with the first iOS release | **Decided 2026-10-16**: universal binary from BI-02. | BP-01 |
| DI3 | iOS minimum version | **Decided 2026-10-16**: the floor of the .NET iOS workload (`SupportedOSPlatformVersion`), verified at BI-01 on the oldest device available; no fixed iOS 15 (`ANALYSIS-B1-MOBILE-SYNC.md` D13 stays unmeasured). Hard lower bounds found: App Store Connect requires iOS 13 or later as the deployment target since 2026-09-09 ([Apple](https://developer.apple.com/news/upcoming-requirements/)); AES-GCM needs iOS 13+ (P12). Microsoft documents iOS 12.2 as the MAUI minimum, so the effective floor is 13 or the workload value if higher. No floor is claimed before TestFlight. | BI-01 |
| DI4 | Bundle identifier | **Decided 2026-10-16**: `com.vger70.medreminder` (independent of the Play application ID). | BI-00 |
| DI5 | Apple-native iCloud transport | Stays excluded (D12). Cloud backup and sync use Google Drive and OneDrive on iOS too. **Decided 2026-10-08.** | - |
| DI6 | Donation links on iOS | **Decided** (DA9, D14): none. | - |
| DI7 | App Store primary category | **Decided 2026-10-17**: Health & Fitness. The review notes of BI-12 still state that the app is not a medical device. | BI-00f, BI-12 |
| DI8 | Framework baseline for the Apple work | **Proposed, to confirm.** Facts: MAUI 10 end of support is 2027-05-11 ([support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/maui)); .NET 11 is at release candidate 2 (published 2026-10-13, with a go-live license) and its general availability is planned for November 2026, no exact day found ([Visual Studio Magazine](https://visualstudiomagazine.com/articles/2026/10/14/net-11-rc2-adds-post-quantum-crypto-maui-csharp-ui-and-nuget-controls.aspx)); MAUI 11 readiness is not confirmed separately; the .NET iOS SDK pins one Xcode version and Apple raises the minimum SDK each spring (Xcode 26 since 2026-04-28), so an iOS build on an old workload may be forced to move in spring 2027 [INFERRED]. Proposed rule: never adopt a preview or release candidate in this project; do not touch the baseline during the current Android test round; make the upgrade (BX-00) the first slice after the Android public launch, only once .NET 11 and MAUI 11 are generally available and have had their first servicing release, and a hard gate before BI-02; if the public launch slips past 2027-03, move BX-00 before it. | BX-00, BI-02 |
| DA3, A6, A7 | Language and catalogue defaults, free apps, private repository | Unchanged; apply to every track. | - |

## 4. Prerequisites (start after the Android public launch, no code)

Every item names what it needs, how it is checked, and the source. The
order is the order of dependency: BX-00 and BI-00a can run in parallel;
everything else needs BI-00a or BI-00b.

### 4.1 Framework baseline

| ID | Item | Detail | Done when |
|---|---|---|---|
| BX-00 | Move the apps to the current MAUI baseline (DI8) | First slice after the Android public launch, before any iOS work; only on generally available releases (no preview or release candidate), after the first servicing release of .NET 11 and MAUI 11; not during the test round, so test results stay valid. MAUI 10 is supported until 2027-05-11 and a major version is supported at least 6 months after its successor ships. Upgrade `net10.0-android` to the .NET 11 target and MAUI 11, and the shared core with it (`core-v*` tag first, then the submodule). Pin the SDK with `global.json` so the Mac, the Windows machines and CI build with the same SDK. Rerun the Android test suite and the pre-release checklist (`docs/RELEASE-CHECKLIST.md`) on the new baseline | Android builds, tests and checklist pass on the new baseline; `global.json` committed |

### 4.2 Apple account and store records (BI-00a, BI-00f)

| ID | Step | Detail | Done when |
|---|---|---|---|
| BI-00a | Apple Account | An Apple Account with two-factor authentication, owned by the person who will own the app | Sign-in to developer.apple.com works |
| BI-00a | Developer Program enrollment (DI1) | Individual enrollment: legal name, government photo ID, annual fee (USD 99 according to third-party guides [UNCERTAIN]; confirm on the Apple enrollment page). Approval can take days | Account active; team ID recorded |
| BI-00f | App Store Connect agreements | Accept the agreements Apple presents; a free app should need no banking or tax setup [UNCERTAIN] | App Store Connect lets you create an app record |
| BI-00f | App record | Name, primary language, bundle ID `com.vger70.medreminder` (DI4, registered first under Identifiers), SKU | Record exists |
| BI-00f | Trader status (DSA) | Declare non-trader (DI1, decided 2026-10-17); email and phone verification codes may still be requested | Declaration complete |
| BI-00f | Age rating | Answer the updated age-rating questionnaire (required by Apple since 2026-01-31) | Rating shown |
| BI-00f | Privacy | Privacy policy URL (the published `PRIVACY.md` and its translations); App Privacy answers drafted from the real data flows (nothing leaves the device unless the user enables a provider) | Answers match `PRIVACY.md`; reviewed at BI-12 |
| BI-00f | Listing texts and assets | Descriptions and keywords in the five languages; support URL; 1024 px app icon; screenshots for the largest iPhone class (6.9 inch) and the 13-inch iPad, which App Store Connect scales to smaller sizes according to third-party guides [UNCERTAIN: confirm in App Store Connect]; primary category Health & Fitness (DI7) | Draft listing complete |
| BI-00f | Export compliance | The app encrypts user data (AES-GCM in the `.mrz` archive and sync). Apple's key is `ITSAppUsesNonExemptEncryption`; it asks about non-exempt encryption, and omitting it triggers a questionnaire at every upload. Do not set it to NO without checking the exemption categories; any national filing (France is often named) is not verified [UNCERTAIN: take advice] | Answer recorded in the release checklist |

### 4.3 Build host (BI-00b)

| Step | Detail | Done when |
|---|---|---|
| Mac | An Apple-silicon Mac on a macOS version that the required Xcode supports ("a Mac that is compatible with Xcode", [Microsoft installation page](https://github.com/dotnet/docs-maui/blob/main/docs/get-started/installation.md)). Disk for Xcode, simulator runtimes and the repository | Mac available to the project |
| Xcode | The .NET iOS SDK requires exactly one Xcode version, and a newer or older one fails the build ([dotnet/maui#34295](https://github.com/dotnet/maui/issues/34295)). Read the version from the release notes of the installed workload (github.com/dotnet/macios/releases; the MAUI "Release Versions" wiki points there). Apple requires uploads built with Xcode 26 or later with the iOS 26 SDK since 2026-04-28 ([Apple](https://developer.apple.com/news/upcoming-requirements/)). Record the pair (workload, Xcode) in the private repository's docs and in CI. Do not set `ValidateXcodeVersion=false` | Xcode launched once; license accepted; iOS simulator runtime installed |
| Command-line tools | `xcode-select --install`; `xcode-select -p` shows the pinned Xcode | Both commands succeed |
| .NET | The SDK of `global.json` (BX-00); `sudo dotnet workload install maui`; `dotnet workload list` ([Microsoft](https://github.com/dotnet/docs-maui/blob/main/docs/get-started/installation.md)) | Workload listed |
| IDE | Visual Studio Code with the .NET MAUI extension on the Mac (".NET MAUI: Configure Apple" refreshes the Apple environment), or Visual Studio on Windows with a networked Mac as build host. Which Visual Studio version builds .NET 10 and 11 is not settled by the sources found [UNCERTAIN]; check the current Microsoft page | One IDE chosen and written down |
| Repository | Clone with submodules (`external/MedReminder`). The private repository uses CRLF files; set `core.autocrlf` or add `.gitattributes` on the Mac to avoid whole-file diffs [INFERRED] | `git status` is clean after a fresh clone |
| Smoke test | An empty MAUI app builds and runs on an iOS simulator, then the project's `Mobile.Core.Tests` run with `dotnet test` | Both pass |

### 4.4 Devices and testers (BI-00c, BT-00)

| ID | Item | Detail | Done when |
|---|---|---|---|
| BI-00c | iPhone | The oldest iPhone available that supports the floor of DI3; Developer Mode enabled; registered by UDID in the account. Simulators do not validate notification delivery, biometrics or background tasks | Debug build installs |
| BI-00c | iPad | One iPad, same steps; ideally a model with Split View and Stage Manager support | Debug build installs |
| BI-00c | TestFlight testers | Internal testers are App Store Connect users with a role; external testers need a beta review [UNCERTAIN: tester limits not verified] | Tester list agreed |
| BT-00 | Android tablets | One 8-inch and one 10-inch tablet; Pixel Tablet, Pixel Fold and a resizable emulator image. Check the effective `targetSdkVersion` of the built manifest: the API 36 override of orientation and resizability applies to apps targeting Android 16. To test it on devices or emulators without targeting API 36, use the "Baklava" preview or the `UNIVERSAL_RESIZABLE_BY_DEFAULT` flag ([Android](https://developer.android.com/develop/adaptive-apps/guides/app-orientation-aspect-ratio-resizability)) | Devices and images available; targetSdk recorded |

### 4.5 Signing and identifiers (BI-00d)

| Step | Detail | Done when |
|---|---|---|
| App ID | Explicit App ID `com.vger70.medreminder` (DI4) with only the capabilities the slices need: Keychain Sharing (MSAL), Background Modes if the identifiers of BI-08 require them, and Time Sensitive Notifications only if BI-04 uses it. No Push, no Associated Domains | App ID saved |
| Certificates | Apple Development (debug) and Apple Distribution (App Store). The private keys and the exported `.p12` stay outside the repository, like the Android upload key (`docs/RELEASE.md`) | Both certificates created; backup of the keys stored |
| Provisioning profiles | One development profile with the registered devices, one App Store distribution profile. Project properties select the signing identity and profile for Release | Signed debug build installs on both devices |
| App Store Connect API key | Issuer ID, key ID and the `.p8` file, for CI upload to TestFlight | Stored as repository secrets, never in files |

### 4.6 OAuth clients (BI-00e)

| Provider | Step | Detail | Done when |
|---|---|---|---|
| Google | iOS client | In the existing Google Cloud project, create an OAuth client of type iOS with the bundle ID. Google iOS clients have no secret; the redirect scheme is the client ID reversed (`com.googleusercontent.apps.<id>`) and is registered in the Info.plist URL types ([AppAuth Google example](https://github.com/valdperformance/AppAuth-iOS/blob/master/Examples/README-Google.md)). Same Drive scopes as Android (`drive.file`, `drive.appdata`); check that the consent screen and its verification cover the new client | Client ID recorded; scheme noted |
| Microsoft | iOS platform | On the desktop's Entra registration add the iOS/macOS platform with the bundle ID. The redirect has the form `msauth.<bundle id>://auth`; the same scheme goes in the Info.plist; MSAL needs a keychain access group enabled in the entitlements and set with `WithIosKeychainSecurityGroup` ([Microsoft Q&A](https://learn.microsoft.com/en-us/answers/questions/595849/unable-to-find-keychain-in-entitlements-plist-usin)). MSAL.NET 4.61 and later no longer support Xamarin iOS, so only the MAUI path applies. Registration needs the tenant owner | Redirect URI registered; no secret involved |

### 4.7 Build and release automation (BI-00g)

| Step | Detail | Done when |
|---|---|---|
| macOS runner | GitHub-hosted `macos-26` images carry Xcode 26.6 as default and 26.5 as well (release notes of 2026-08-05, seen through a mirror [UNCERTAIN]). Select the Xcode pinned in 4.3 with `xcode-select` or `DEVELOPER_DIR`; a runner whose default differs from the workload's Xcode fails the build. macOS minutes cost more than Linux minutes; check the plan quota [UNCERTAIN] | A workflow builds the empty iOS app on the runner |
| Workflow | A separate `ios.yaml`, not a required check at first (the required `Android build` check stays untouched). Signing material comes from secrets into a temporary keychain; the same submodule-pin check as `android.yaml` applies | Dispatch run produces a signed `.ipa` |
| Package check | A `build/check-ios-package.sh` counterpart of `check-android-package.sh`: no `.pdb`/`.xml`, bundle ID, `PrivacyInfo.xcprivacy` present, Info.plist keys of the slices present | Script passes on the `.ipa` |

## 5. Backlog by track

### BT - Android tablets

**Entry:** DT1-DT3 decided (done 2026-10-16); the Android public launch
gate passed. **Exit:** every screen of M1-M5
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
| BT-03 | Wide navigation | Bottom tabs on compact; from medium, the locked Shell flyout lists the same four destinations (DT2); `AppShell` builds its items in code, so a change of class (rotation, resize, fold) rebuilds them without losing the current page, its form input or the back stack [INFERRED: Shell items are not switchable in place]; screen-reader order matches visual order | BT-01, DT2 |
| BT-04 | List-detail panes | Medicines, Agenda and History show list and detail side by side from the expanded class; the detail content is a reusable view (today `MedicineDetailPage` and `HistoryPage` are pushed with constructor arguments), the compact layout keeps push navigation; the unsaved-changes guard now in `FormPage.OnBackButtonPressed` moves into the reusable view so that selecting another list item, rotating or resizing is also guarded; selection and form state survive rotation and resize | BT-01, BT-02 |
| BT-05 | Camera, pairing and sharing on tablets | QR pairing (the 260 x 260 code in `HouseholdPages.cs`) and barcode scan work with a front-only camera or no camera (the camera permission is asked only when a scan page opens, and the manifest declares the camera optional); `.mrz` export/import through the document picker, and the calendar file and prescription text through the share sheet, work in split-screen; app lock and reminders behave as on phones | BT-02 |
| BT-06 | Tablet verification matrix | Results recorded for: Pixel Tablet and foldable emulators, both orientations, split-screen, free-form window, font and display-size scaling, hardware keyboard focus order, the two real tablets; no overlap, clipping or lost input | BT-02..05, BT-00 |
| BT-07 | Play listing and guides | 7-inch and 10-inch screenshots; user guides in all five languages describe the wide layout; Play Console large-screen report reviewed [UNCERTAIN: Google's current large-screen quality criteria were not verified for this document] | BT-06 |

Effort: about 14-22 days [INFERRED]; BT-04 is the largest item.

### BI - iOS (iPhone)

**Entry:** BI-00a..g, BX-00 and DI8 done; the Android public launch gate passed (DA4).
**Exit:** the manual checklist of the B.1 plan passes on a supported
iPhone from TestFlight, and App Store review passes.

| ID | Slice | Acceptance / done when | Depends on |
|---|---|---|---|
| BI-01 | iOS spikes | S3: EF Core and Microsoft.Data.Sqlite run on a device under iOS AOT with the Release defaults (iOS has no JIT, so reflection-based EF Core and `System.Text.Json` may need trimmer roots or source generation); S1: AES-GCM works (iOS 13+ per P12); S5: local notifications fire on time, and the pending limit is measured (an Apple engineer states 64 pending requests per app on the [developer forums](https://developer.apple.com/forums/thread/811171); the documentation page itself was not retrieved); S9: the in-process restart design of BI-09; S10: the unsaved-changes guard on the iOS back button (BI-11); the oldest supported iOS version (DI3) is exercised. Results recorded in `ANALYSIS-B1-MOBILE-SYNC.md` §18 style | BI-00b, BI-00c |
| BI-02 | Multi-target skeleton | `net10.0-android;net10.0-ios` (or the BX-00 target) with the iOS target conditional on macOS so Linux and Windows builds are unchanged; the `#if ANDROID` registration of `MauiProgram.cs` moves into per-platform partial classes; Android-only package references become conditional on the target framework; `Platforms/iOS/` exists; `Platform` becomes `"ios"` at both places it is set (`MobileServices.cs` default and `MauiProgram.cs`), and the device list and sync pages are checked for a label for `"ios"`; no shared-core change is expected because the core treats `Platform` as free text (to confirm, then change in this repository first if it is not); `CheckSharedCoreBoundary` and the strip target still pass; the `Android build` check is unaffected | BI-01, BX-00 |
| BI-03 | App data, backup exclusion and file protection | Databases and settings stay in the sandbox; the profile directories are excluded from iCloud/device backup and carry a file-protection class chosen with the lock-state trade-off for background sync stated; exact APIs confirmed in BI-01 [UNCERTAIN]. A restore from `.mrz` and an upgrade keep the data | BI-02 |
| BI-04 | Reminders | `UNUserNotificationCenter` adapter behind the existing `IWindowsNotificationService`, `IReminderScheduler` and `IReminderSettings` ports (the `DeviceNotificationFilter` wrapper keeps working): authorization request and denial handling; the direct `Permissions.PostNotifications` calls in `Onboarding.cs` and `HouseholdPages.cs` move behind the port; the planner's rolling window within the measured pending limit and the closing "open MedReminder to keep reminders active" notice; snooze and open-medicine actions that never record an intake silently; lock-screen detail hidden by default (B2-09); iOS has no notification channels, exact-alarm permission or boot event, so `ReminderStatus` is filled with exact-alarm not applicable, channel flags from the authorization settings, background restricted when Background App Refresh is off, and reminders are re-planned on launch, on foreground and in background refresh; the exact-alarm and battery cards are hidden; a Time Sensitive level is used only if a capability is justified [UNCERTAIN] | BI-02, BI-01 (S5) |
| BI-05 | Documents, share and URLs | `UIDocumentPickerViewController` behind `IDocumentPicker` for `.mrz` export and import; MAUI `Share` for the calendar file and the prescription text (popover anchors in BP-02); `Launcher` for mailto and regional-service links (`LSApplicationQueriesSchemes` where a scheme is probed); `RegionalServiceView.AppUrl` picks `IosAppUrl` on iOS (the domain and the feed already carry it; today the view returns only `AndroidAppUrl`) | BI-02 |
| BI-06 | Security | `LocalAuthentication` app lock with passcode fallback (`NSFaceIDUsageDescription`); a Keychain-backed `ICredentialProtector`; no secret in logs; the Keychain item accessibility class stated and tested across reboot and restore | BI-02 |
| BI-07 | Cloud providers | Google Drive: Android's Identity Services client does not exist on iOS. Spike two routes, then choose: (a) a code-and-PKCE flow through MAUI `WebAuthenticator` (system authentication session) with the reversed-client-ID scheme, tokens kept through the Keychain-backed `ICredentialProtector`, refresh handled by the app; (b) a maintained .NET binding of AppAuth or Google Sign-In, whose availability is not verified [UNCERTAIN]. Custom-scheme callbacks were hijackable before iOS 17.4 ([OSW 2024](https://aaronparecki.com/2024/04/11/23/2024-04-11%20OSW%20Native%20Apps%20Presented.pdf)); PKCE does not cover that attack, so record the residual risk in the privacy review. OneDrive: MSAL with the keychain group, `OpenUrl` forwarded to `AuthenticationContinuationHelper`, redirect `msauth.<bundle id>://auth`. One cloud account at a time as on Android; backup and restore of the same encrypted archive | BI-02, BI-06, BI-00e |
| BI-08 | Background work | `BGTaskScheduler` for sync, cloud backup and catalogue refresh (the three Android schedulers `ISyncJobScheduler`, `ICloudBackupScheduler`, `ICatalogueRefreshScheduler` become iOS adapters): identifiers in `BGTaskSchedulerPermittedIdentifiers` and the matching `UIBackgroundModes`; handlers registered at launch; `earliestBeginDate` is a lower bound, the system decides when to run, and a refresh task runs for a short time only (about 30 s according to a third-party source [UNCERTAIN]); submission can fail when Background App Refresh is off. Sync on foreground and after local writes stays the primary path; the UI states the limit like the Android battery guidance (B2-10), without a settings route that does not exist | BI-02, BI-07 |
| BI-09 | In-process restart after join | iOS cannot restart the app and terminating it from code is discouraged [UNCERTAIN: confirm in the review guidelines]. The installation join (`IAppRestarter`) therefore tears down the profile-dependent services and rebuilds the shell in process; no singleton keeps the old profile; as `AndroidAppRestarter` does, the notifications of the previous profile are removed (pending and delivered) before the new profile plans its own. Design decided in spike S9 | BI-02, BI-01 (S9) |
| BI-10 | Camera features | QR pairing and barcode scan through ZXing.Net.Maui on iOS: `NSCameraUsageDescription` in the Info.plist (documented by the library), the runtime request stays in the app's existing `Permissions.Camera` calls; denial and no-camera states; the camera is requested only when a scan page opens. QR generation (`BarcodeGeneratorView`) on iOS is not documented in the sources found and is checked on a device [UNCERTAIN] | BI-02 |
| BI-11 | Platform polish | System language from `CultureInfo` (already used) and region through an iOS `IDeviceRegion` check (`SystemDeviceRegion` is the default today) for the DA3 preselection; dark mode; Dynamic Type; VoiceOver labels and focus order; the shell's tab bar with the current iOS styling; the PDF (SkiaSharp) renders with the embedded fonts; the unsaved-changes guard (`FormPage.OnBackButtonPressed`) also covers the navigation-bar back button and the swipe-back gesture, which may bypass that override on iOS ([Microsoft Q&A](https://learn.microsoft.com/en-us/answers/questions/2149971/when-using-navigationpage-as-the-main-page-i-do-no); spike S10) | BI-02, BI-01 (S10) |
| BI-12 | Store readiness and TestFlight | `PrivacyInfo.xcprivacy` in `Platforms/iOS` (every MAUI iOS app needs one; baseline reasons file timestamp C617.1, system boot time 35F9.1, disk space E174.1, plus user defaults C56D.1 if `Preferences` or `NSUserDefaults` is used; [Microsoft](https://learn.microsoft.com/dotnet/maui/ios/privacy-manifest), lists verified for MAUI 8 and later) with the App Store privacy answers matching the real flows; export compliance decided in BI-00f and `ITSAppUsesNonExemptEncryption` set accordingly; review notes explaining the non-medical-device positioning and giving a way to test without an account; the build uses Xcode 26 or later with the iOS 26 SDK and targets iOS 13 or later (Apple, 2026); the age rating and trader status of BI-00f are complete; `build/check-ios-package.sh` (BI-00g) passes; the macOS workflow signs and uploads to TestFlight with the App Store Connect API key; internal then external TestFlight round with the same manual checklist as Android | BI-03..11, BI-00f, BI-00g |
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
| BP-02 | Popover anchors and sheets | Share sheet (`Share.Default.RequestAsync` in the calendar and prescription pages), the document picker and any menu present as popovers anchored to the tapped control on iPad, using the share request's source-bounds option [UNCERTAIN: member name to confirm]; a missing anchor crashes on iPad [UNCERTAIN] | BI-05 |
| BP-03 | Multitasking and windows | Split View, Slide Over and resizable windows keep state through resize; the scene-lifecycle requirement of the Apple SDK is checked against the live Apple documentation (the Upcoming Requirements page of 2026-10-17 lists no such requirement) and adopted if it applies [UNCERTAIN]; the app does not claim full-screen-only | BP-01 |
| BP-04 | Keyboard and pointer | Hardware keyboard focus order and shortcuts for the main destinations; pointer hover states; text fields and pickers usable with the keyboard | BP-01 |
| BP-05 | iPad verification and store assets | Real-iPad run of the manual checklist; 13-inch iPad screenshots (2064 x 2752 according to third-party guides [UNCERTAIN: confirm in App Store Connect]); TestFlight round on iPad | BP-01..04, BI-12 |

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

1. After the Android public launch gate (decided 2026-10-16): start
   BX-00, BI-00a..g and BT-00. DT1-DT3 and DI1-DI4 are settled; confirm
   DI8 first. Starting later moves the first iOS spike by the time enrollment and
   device delivery take [INFERRED: days to weeks].
2. Run BI-01 as soon as the Mac and devices exist. A failed S3, S5 or S9 on
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

## 9. Verification record of section 5 (2026-10-17)

Method: each step was read against `vger70/medreminder-mobile` at
`bf8d401` and `vger70/MedReminder` at the head of `main`, and against
vendor pages where reachable. "Confirmed" means the cited code or page
supports the step as written; "Corrected" means the step was changed in
section 5; "Unverified" means the tag stays.

| Step | Result | Basis |
|---|---|---|
| BT-01 | Confirmed | No size-class or idiom code exists in `Pages/`; `Mobile.Core` is MAUI-free, so a pure type fits |
| BT-02 | Confirmed | `Ui.Stack()` and `Ui.Scroll()` have no width cap; `Ui.TwoColumns` exists |
| BT-03 | Corrected | `AppShell` builds a `TabBar` in code; switching to a flyout at run time needs a rebuild that keeps page state |
| BT-04 | Corrected | Detail pages are pushed with constructor arguments; the unsaved-changes guard lives in `FormPage.OnBackButtonPressed` and must move with the view |
| BT-05 | Corrected | 260 x 260 QR confirmed; sharing goes through `Share`, `.mrz` through the document picker; camera permission requested only on scan pages |
| BT-06 | Confirmed | Matches the API 36 behavior; emulator and flag options cited in 4.4 |
| BT-07 | Unverified | Google's current large-screen quality criteria were not retrieved |
| BI-01 | Corrected | 64-request limit now backed by an Apple engineer on the forums, documentation page not retrieved; spikes S9 and S10 added |
| BI-02 | Corrected | `Platform` is set in two places; the core treats it as free text; no shared-core change expected; package check script added in 4.7 |
| BI-03 | Unverified | Backup-exclusion and file-protection APIs are not confirmed from sources; resolved by BI-01 |
| BI-04 | Corrected | Ports confirmed; `ReminderStatus` and four `ReminderChannel` values assume Android channels and alarms; permission calls bypass the port |
| BI-05 | Corrected | `IosAppUrl` exists in the domain and feed, but the mobile view returns only `AndroidAppUrl`; sharing is not the document picker |
| BI-06 | Confirmed | `IAppLock` and `ICredentialProtector` are the Android-registered ports; Keychain and LocalAuthentication are the iOS counterparts |
| BI-07 | Corrected | The Android Google client has no iOS equivalent; two routes to be spiked; MSAL iOS redirect and keychain steps confirmed from Microsoft pages; MAUI-specific MSAL steps not retrieved |
| BI-08 | Confirmed | Three Android schedulers map to `BGTaskScheduler`; identifiers, background modes and non-guaranteed timing confirmed from secondary sources; .NET binding details not retrieved |
| BI-09 | Corrected | `AndroidAppRestarter` also cancels all notifications; iOS needs the same cleanup and an in-process design (spike S9) |
| BI-10 | Corrected | `NSCameraUsageDescription` confirmed from the library README; QR generation on iOS not documented in sources found |
| BI-11 | Corrected | Region goes through `IDeviceRegion`; back-button guard issue on iOS added |
| BI-12 | Corrected | Apple SDK minimum (Xcode 26, since 2026-04-28), deployment target 13 (since 2026-09-09), age rating (2026-01-31) and privacy manifest reasons added |
| BI-13 | Confirmed | Guides and checklists are the files in `docs/` of the private repository |
| BP-01 | Confirmed | Depends on the same adaptive layer; DI2 decided |
| BP-02 | Corrected | The share sheet call sites are named; the exact MAUI member for the anchor is not verified |
| BP-03 | Corrected | Apple's Upcoming Requirements page shows no scene-lifecycle requirement; kept as a check |
| BP-04 | Unverified | Keyboard and pointer behavior of MAUI on iPad not researched |
| BP-05 | Corrected | iPad screenshot size taken from third-party guides |

Sources added by this verification:
- [.NET MAUI support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/maui)
- [Apple upcoming requirements](https://developer.apple.com/news/upcoming-requirements/)
- [Apple DSA trader information](https://developer.apple.com/help/app-store-connect/manage-compliance-information/manage-european-union-digital-services-act-compliance-information)
- [Microsoft MAUI supported platforms and installation](https://github.com/dotnet/docs-maui/blob/main/docs/get-started/installation.md)
- [Apple developer forum on the 64 pending notifications](https://developer.apple.com/forums/thread/811171)
- [Microsoft MAUI Apple privacy manifest](https://learn.microsoft.com/dotnet/maui/ios/privacy-manifest)
