# Copilot Instructions for MauiAppUpdaterPlugin

## Project Overview
This is a .NET MAUI plugin for handling in-app updates across different platforms (iOS and Android). The plugin provides a unified interface for checking and applying updates while implementing platform-specific behaviors.

## Key Architecture Components

### Core Interfaces and Types
- `IAppUpdater` (`src/MauiAppUpdater/IAppUpdater.cs`) - Main interface defining update operations
- `UpdateInfo` record - Represents update status and metadata
- `UpdateType` enum - Defines update types: None, Flexible, Immediate

### Platform-Specific Implementations
- Android: `src/MauiAppUpdater/Platforms/Android/AppUpdater_Android.cs`
- iOS: `src/MauiAppUpdater/Platforms/iOS/AppUpdater_iOS.cs`

## Development Patterns

### Cross-Platform Development
1. Define interfaces in the root namespace
2. Implement platform-specific logic in `Platforms/{Android|iOS}` folders
3. Use platform conditionals (`#if __ANDROID__` etc.) for platform-specific code

### Testing Conventions
- Unit tests use xUnit with theory-based testing for validation rules
- Test files mirror the structure of source files
- Example: `ValidatorTests.cs` tests input validation with multiple test cases

### Integration Points
- Android: Play Store integration via package name
- iOS: App Store integration via Apple ID or URL
- Sample app demonstrates integration patterns in `MauiSampleApp`

## Critical Workflows
1. Building:
   ```
   dotnet build MauiAppUpdaterPlugin_final_full.sln
   ```
2. Running tests:
   ```
   dotnet test tests/MauiAppUpdater.Tests/MauiAppUpdater.Tests.csproj
   ```

## Code Generation Patterns
- Use records for immutable data structures (e.g., `UpdateInfo`)
- Follow MAUI dependency injection patterns in `MauiAppUpdaterExtensions.cs`
- Platform-specific implementations should implement `IAppUpdater`

## Common Tasks
- Adding new update types: Extend `UpdateType` enum and implement in platform-specific classes
- Platform-specific features: Add to relevant `AppUpdater_{Platform}.cs` file
- UI customization: Modify `UI/iOSUpdatePage.xaml` for iOS update dialogs