# Maui.AppUpdater

[![Build and Test](https://github.com/Aqsa-Aftab/Maui.AppUpdater/actions/workflows/build.yml/badge.svg)](https://github.com/Aqsa-Aftab/Maui.AppUpdater/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Maui.AppUpdater.svg)](https://www.nuget.org/packages/Maui.AppUpdater/)
[![License](https://img.shields.io/github/license/Aqsa-Aftab/Maui.AppUpdater.svg)](LICENSE)

> ⚠️ **Project Status**: In Development - Not ready for production use

## Compatibility

### Supported .NET Versions
- ✅ .NET 9.0 (Current - Latest Features)
- ✅ .NET 8.0 (LTS)
- ✅ .NET 7.0
- ❌ .NET 6.0 and earlier

### Supported Platforms
- ✅ Android 7.0 (API 24) or higher
- ✅ iOS 14.0 or higher
- ✅ macOS 11.0 or higher (via Mac Catalyst)
- ✅ Windows 10.0.19041.0 or higher

### Required Development Tools
- Visual Studio 2025+ or Visual Studio Code
- .NET 9 SDK
- MAUI Workload (`dotnet workload install maui`)

A powerful and flexible in-app update solution for .NET MAUI applications. Seamlessly handle app updates across iOS and Android platforms with a unified API.

[![Buy Me A Coffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/yourusername)

## Features

- ✨ Cross-platform support (iOS & Android)
- 🔄 Flexible and immediate update types
- 📱 Native integration with App Store and Play Store
- 🎨 Customizable update UI
- ⚡ Async/await support
- 🔒 Built-in validation
- 📊 Update status tracking

## Installation

```bash
dotnet add package MauiAppUpdater
```

## Quick Start

1. **Configure Services** (in `MauiProgram.cs`):
```csharp
builder.Services.AddMauiAppUpdater(options =>
{
    options.PlayStorePackageName = "com.yourcompany.app";  // Android
    options.AppStoreIdOrUrl = "1234567890";               // iOS
});
```

2. **Check for Updates**:
```csharp
public partial class MainPage : ContentPage
{
    private readonly IAppUpdater _appUpdater;

    public MainPage(IAppUpdater appUpdater)
    {
        InitializeComponent();
        _appUpdater = appUpdater;
    }

    private async Task CheckForUpdate()
    {
        var updateInfo = await _appUpdater.CheckForUpdateAsync();
        if (updateInfo.IsUpdateAvailable)
        {
            if (updateInfo.Type == UpdateType.Flexible)
            {
                await _appUpdater.StartFlexibleUpdateAsync();
            }
            else
            {
                await _appUpdater.StartImmediateUpdateAsync();
            }
        }
    }
}
```

## Advanced Configuration

```csharp
builder.Services.AddMauiAppUpdater(options =>
{
    // Store Configuration
    options.PlayStorePackageName = "com.yourcompany.app";
    options.AppStoreIdOrUrl = "1234567890";

    // Update Strategy
    options.UpdateStrategy = UpdateStrategy.Automatic;
    options.ForceUpdate = true;
    options.CheckInterval = TimeSpan.FromHours(12);

    // UI Customization
    options.LogoImageResource = "update_logo";
    options.PrimaryColor = "#007AFF";
});
```

## Platform-Specific Features

### Android
- Flexible updates with background download
- Immediate updates with forced installation
- Play Store fallback for manual updates
- Update priority support

### iOS
- App Store version checking
- Automatic store redirect
- Release notes support
- Version comparison

## Error Handling

The plugin includes built-in error handling and validation:

```csharp
try
{
    var updateInfo = await _appUpdater.CheckForUpdateAsync();
    // Handle update
}
catch (AppUpdateException ex)
{
    // Handle specific update errors
    Debug.WriteLine($"Update error: {ex.Message}");
}
catch (Exception ex)
{
    // Handle general errors
    Debug.WriteLine($"Error: {ex.Message}");
}
```

## Best Practices

1. **Update Frequency**
   - Don't check for updates on every app launch
   - Use the `CheckInterval` setting to limit checks
   - Consider user's data connection

2. **User Experience**
   - Use flexible updates for non-critical changes
   - Reserve immediate updates for security fixes
   - Provide clear update descriptions

3. **Error Handling**
   - Always implement proper error handling
   - Provide fallback options
   - Log update failures for debugging

## Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details.

## Support the Project

If you find this plugin helpful, consider buying me a coffee! Your support helps maintain and improve the project.

[![Buy Me A Coffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/yourusername)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.