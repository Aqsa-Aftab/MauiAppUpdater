# MauiAppUpdater Documentation

## Table of Contents

1. [Version Support](#version-support)
2. [Architecture](#architecture)
3. [Core Components](#core-components)
4. [Implementation Guide](#implementation-guide)
5. [API Reference](#api-reference)
6. [Platform-Specific Details](#platform-specific-details)
7. [Troubleshooting](#troubleshooting)

## Version Support

### .NET Support Matrix

| .NET Version | Status | Support Timeline | Feature Set |
|--------------|--------|------------------|--------------|
| .NET 9.0     | ✅ Full | Current (2025) | Full + Latest Features |
| .NET 8.0     | ✅ Full | Until Nov 2026 (LTS) | Full |
| .NET 7.0     | ✅ Full | Until May 2024 | Full |
| .NET 6.0     | ❌     | Not Supported | N/A |

### Feature Availability

All core features are available across .NET 7.0, 8.0, and 9.0, including:
- App Store/Play Store integration
- Flexible and immediate updates
- Version checking
- Update prompts
- Background downloads

#### Version-Specific Optimizations
- .NET 9.0: Latest performance improvements and platform features
- .NET 8.0: Optimized for long-term stability
- .NET 7.0: Fully functional with baseline performance

### Platform Version Requirements

#### Android
- Minimum API Level: 24 (Android 7.0)
- Target API Level: 34 (Android 14.0)
- Required Features:
  - Google Play Services
  - Play Store installed
  - Internet connectivity

#### iOS
- Minimum Version: iOS 14.0
- Recommended: iOS 16.0 or higher
- Required Capabilities:
  - App Store access
  - Internet connectivity

#### Windows
- Minimum Version: Windows 10.0.19041.0
- Recommended: Windows 11
- Required Features:
  - Microsoft Store access
  - Internet connectivity

#### MacOS (via Mac Catalyst)
- Minimum Version: macOS 11.0
- Recommended: macOS 13.0 or higher
- Required Features:
  - App Store access
  - Internet connectivity

### Development Environment

#### Required Tools
- Visual Studio 2025+ (Windows/Mac) or Visual Studio Code
- .NET 9 SDK
- MAUI Workload
```bash
dotnet workload install maui
```

#### Optional Tools
- Android Studio (for advanced Android debugging)
- Xcode 17+ (for iOS/macOS development)
- Windows App SDK (for Windows development)

## Architecture

### Overview

MauiAppUpdater follows a layered architecture:

```
┌─────────────────┐
│     App UI      │
├─────────────────┤
│   IAppUpdater   │
├────────┬────────┤
│ Android │  iOS  │
└────────┴────────┘
```

### Core Components

- `IAppUpdater`: Main interface for update operations
- `AppUpdaterOptions`: Configuration container
- Platform implementations:
  - `AppUpdater_Android`: Android-specific implementation
  - `AppUpdater_iOS`: iOS-specific implementation

## Implementation Guide

### 1. Basic Setup

```csharp
// MauiProgram.cs
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    
    builder.Services.AddMauiAppUpdater(options =>
    {
        options.PlayStorePackageName = "com.company.app";
        options.AppStoreIdOrUrl = "1234567890";
    });
    
    return builder.Build();
}
```

### 2. Version Check Implementation

```csharp
public class UpdateService
{
    private readonly IAppUpdater _appUpdater;
    
    public UpdateService(IAppUpdater appUpdater)
    {
        _appUpdater = appUpdater;
    }
    
    public async Task CheckForUpdates()
    {
        var updateInfo = await _appUpdater.CheckForUpdateAsync();
        
        if (updateInfo.IsUpdateAvailable)
        {
            switch (updateInfo.Type)
            {
                case UpdateType.Flexible:
                    await HandleFlexibleUpdate(updateInfo);
                    break;
                case UpdateType.Immediate:
                    await HandleImmediateUpdate(updateInfo);
                    break;
            }
        }
    }
    
    private async Task HandleFlexibleUpdate(UpdateInfo info)
    {
        // Implement flexible update logic
        await _appUpdater.StartFlexibleUpdateAsync();
    }
    
    private async Task HandleImmediateUpdate(UpdateInfo info)
    {
        // Implement immediate update logic
        await _appUpdater.StartImmediateUpdateAsync();
    }
}
```

## API Reference

### IAppUpdater Interface

```csharp
public interface IAppUpdater
{
    Task<UpdateInfo> CheckForUpdateAsync();
    Task<bool> StartFlexibleUpdateAsync();
    Task<bool> StartImmediateUpdateAsync();
    Task<bool> PromptToOpenStoreAsync();
}
```

### UpdateInfo Record

```csharp
public record UpdateInfo(
    bool IsUpdateAvailable,
    string LatestVersion,
    UpdateType Type,
    int Priority,
    string? ReleaseNotes = null
);
```

### Configuration Options

```csharp
public class AppUpdaterOptions
{
    public string? PlayStorePackageName { get; set; }
    public string? AppStoreIdOrUrl { get; set; }
    public UpdateStrategy UpdateStrategy { get; set; }
    public bool ForceUpdate { get; set; }
    public TimeSpan CheckInterval { get; set; }
    public string? LogoImageResource { get; set; }
    public string? PrimaryColor { get; set; }
}
```

## Platform-Specific Details

### Android Implementation

- Uses Google Play Core API for update management
- Supports flexible and immediate updates
- Handles background downloads and installation

Required Android Manifest Permissions:
```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.REQUEST_INSTALL_PACKAGES" />
```

### iOS Implementation

- Uses iTunes Search API for version checks
- Implements native App Store redirection
- Handles version comparison and update prompts

Required Info.plist Entry:
```xml
<key>LSApplicationQueriesSchemes</key>
<array>
    <string>itms-apps</string>
</array>
```

## Troubleshooting

### Common Issues

1. **Update Check Fails**
   - Verify internet connectivity
   - Confirm store IDs are correct
   - Check platform-specific configuration

2. **Version Comparison Issues**
   - Ensure version strings follow semantic versioning
   - Verify store metadata is correct

3. **Update Installation Fails**
   - Check device storage space
   - Verify permissions
   - Review system logs

### Debugging

Enable debug logging:
```csharp
builder.Services.AddMauiAppUpdater(options =>
{
    options.EnableDebugLogging = true;
});
```

## Development: testing updates locally (MockAppUpdater)

To exercise the update UI and flows during development without hitting the real App Store or Play Store, a mock implementation of `IAppUpdater` is provided for the sample app.

- File: `src/Samples/MauiSampleApp/Mocks/MockAppUpdater.cs`
- The mock returns a simulated `UpdateInfo` (update available) and simulates success for update actions.

How to use

1. The sample app registers the mock automatically when building in `DEBUG` mode. Open `src/Samples/MauiSampleApp/MauiProgram.cs` and you'll find:

```csharp
// DEBUG-only registration (already added in the sample app)
#if DEBUG
builder.Services.AddSingleton<MauiAppUpdater.IAppUpdater, MockAppUpdater>();
#endif
```

2. Run the app on simulator/emulator (iOS simulator or Android emulator). The app will behave as if an update is available and will show the update UI.

Commands (examples):

```bash
# iOS simulator (from repo root)
dotnet build -f net9.0-ios src/Samples/MauiSampleApp/MauiSampleApp.csproj
# optionally install+launch using simctl if dotnet's Run picks an unsupported runtime
xcrun simctl install <UDID> src/Samples/MauiSampleApp/bin/Debug/net9.0-ios/iossimulator-arm64/MauiSampleApp.app
xcrun simctl launch <UDID> com.example.mauisample

# Android emulator
dotnet build -f net9.0-android src/Samples/MauiSampleApp/MauiSampleApp.csproj
# then run apk on emulator via adb or let dotnet run pick emulator
```

When to use real store testing

- Android: use Play Console internal testing or internal app sharing to validate the Play Core in-app update flows end-to-end. Upload a new build with an increased `versionCode` to trigger Play-side updates.
- iOS: use TestFlight via App Store Connect to validate App Store/TestFlight update flows. Upload a new build with a higher `CFBundleShortVersionString`/`CFBundleVersion` and distribute to testers.

Notes and best practices

- Mocking is ideal for exercising UI, logic, and developer workflows quickly. Use real store testing to validate store-side behavior (metadata, asset packaging, Play Core interactions, TestFlight behavior).
- Ensure the sample app's bundle id/package name matches the store entry when doing real store testing.
- If you need a different mock behavior (Immediate update, no update available, etc.), modify `MockAppUpdater.cs` or add a simple configuration switch (environment variable, debug UI) to change the returned `UpdateInfo`.

### What happens when the user taps "Update now"

- iOS: The in-app prompt implemented in `iOSUpdatePage` calls `IAppUpdater.PromptToOpenStoreAsync()` which is expected to open the App Store (or TestFlight) page for the app (via a URL / Launcher). After the store is opened the update page closes. In our mock, this call simply returns true to simulate success.
- Android: Android's UI typically uses the Play Core in-app update APIs. For the flexible flow the app will start a background download and allow the user to install the update later; for the immediate flow the Play Core flow takes over and forces the update flow. In this codebase the Android-specific updater implements those flows; during development the mock will return success immediately.

If you want to fully simulate the App Store/Play Store experience you must use TestFlight (iOS) or Play Console internal testing / internal app sharing (Android) and upload builds with incremented version numbers.


## Support

For bug reports and feature requests, please use the GitHub Issues section.

For premium support or consulting, please [Buy Me a Coffee](https://www.buymeacoffee.com/yourusername) and reach out via email.