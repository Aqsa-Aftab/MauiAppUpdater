# Development Environment Setup

This guide will help you set up your development environment for working with MauiAppUpdater.

## Prerequisites

1. **Required Software**
   - [Visual Studio 2025+](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
   - [.NET 9 SDK](https://dotnet.microsoft.com/download)
   - [Git](https://git-scm.com/)

2. **MAUI Workload**
   ```bash
   dotnet workload install maui
   ```

3. **Platform-Specific Requirements**

   **iOS Development:**
   - macOS machine with latest Xcode
   - iOS development certificates
   - Provisioning profiles

   **Android Development:**
   - Android SDK
   - Android Emulator or physical device
   - Java Development Kit (JDK)

## Getting Started

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Aqsa-Aftab/MauiAppUpdater.git
   cd MauiAppUpdater
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the Solution**
   ```bash
   dotnet build
   ```

4. **Run Tests**
   ```bash
   dotnet test
   ```

## IDE Setup

### Visual Studio
1. Open `Maui.AppUpdater.sln`
2. Ensure Platform targets are set correctly
3. Build and run

### Visual Studio Code
1. Install C# extension
2. Open workspace
3. Configure launch settings
4. Build and debug

## Sample App

1. Open `src/Samples/MauiSampleApp`
2. Configure store IDs in `MauiProgram.cs`
3. Run on desired platform

## Debugging Tips

1. **Android**
   - Use Android Debug Bridge (adb)
   - Enable developer options on device
   - Monitor Google Play Services

2. **iOS**
   - Use Xcode debugging tools
   - Monitor device logs
   - Test on real devices

## Common Issues

1. **Build Errors**
   - Clean solution
   - Delete bin/obj folders
   - Restore NuGet packages

2. **Platform Issues**
   - Verify SDK installations
   - Check platform-specific setup
   - Validate store configurations

## Additional Resources

- [MAUI Documentation](https://docs.microsoft.com/dotnet/maui)
- [Android In-App Updates](https://developer.android.com/guide/playcore/in-app-updates)
- [iOS App Store](https://developer.apple.com/app-store/)