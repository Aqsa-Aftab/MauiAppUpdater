# Mock Implementation Guide

This document describes how to use and extend the mock implementation of MauiAppUpdater for testing and development.

## Overview

The mock implementation (`MockAppUpdater`) allows developers to test update flows without connecting to actual app stores. This is particularly useful for:
- Local development
- UI testing
- Integration testing
- Demo purposes

## Using the Mock Implementation

1. **Register the Mock**
```csharp
#if DEBUG
builder.Services.AddSingleton<IAppUpdater, MockAppUpdater>();
#endif
```

2. **Configure Behavior**
```csharp
public class MockAppUpdater : IAppUpdater
{
    public Task<UpdateInfo> CheckForUpdateAsync()
    {
        return Task.FromResult(new UpdateInfo(
            IsUpdateAvailable: true,        // Simulate update available
            LatestVersion: "2.0.0",        // Set mock version
            Type: UpdateType.Flexible,      // Choose update type
            Priority: 1,                    // Set priority
            ReleaseNotes: "Test update"    // Add notes
        ));
    }

    // Implement other methods...
}
```

3. **Test Different Scenarios**
```csharp
// No update available
IsUpdateAvailable: false

// Force immediate update
Type: UpdateType.Immediate

// High priority update
Priority: 5

// Failed update
throw new AppUpdateException("Test error");
```

## Testing Guidelines

1. **Test Update Types**
   - Test both flexible and immediate updates
   - Verify update flow transitions
   - Check error handling

2. **Test User Interactions**
   - Update acceptance
   - Update rejection
   - Remind later functionality
   - Progress indicators

3. **Test Error Scenarios**
   - Network errors
   - Version parsing errors
   - Installation failures

## Example Test Cases

```csharp
public class UpdateTests
{
    private readonly MockAppUpdater _updater;

    public UpdateTests()
    {
        _updater = new MockAppUpdater();
    }

    [Fact]
    public async Task TestUpdateAvailable()
    {
        var result = await _updater.CheckForUpdateAsync();
        Assert.True(result.IsUpdateAvailable);
    }

    [Fact]
    public async Task TestUpdateType()
    {
        var result = await _updater.CheckForUpdateAsync();
        Assert.Equal(UpdateType.Flexible, result.Type);
    }
}
```

## Extending the Mock

1. **Add Configuration Options**
```csharp
public class MockAppUpdater : IAppUpdater
{
    private readonly bool _simulateError;
    private readonly UpdateType _updateType;

    public MockAppUpdater(bool simulateError = false, 
                         UpdateType updateType = UpdateType.Flexible)
    {
        _simulateError = simulateError;
        _updateType = updateType;
    }

    // Implementation...
}
```

2. **Add Progress Simulation**
```csharp
public class MockAppUpdater : IAppUpdater
{
    public event EventHandler<double> DownloadProgress;

    private async Task SimulateDownloadProgress()
    {
        for (int i = 0; i <= 100; i += 10)
        {
            DownloadProgress?.Invoke(this, i / 100.0);
            await Task.Delay(500);
        }
    }
}
```

## Integration Example

```csharp
public class UpdateService
{
    private readonly IAppUpdater _updater;
    private readonly ILogger _logger;

    public UpdateService(IAppUpdater updater, ILogger logger)
    {
        _updater = updater;
        _logger = logger;
    }

    public async Task CheckForUpdates()
    {
        try
        {
            var info = await _updater.CheckForUpdateAsync();
            if (info.IsUpdateAvailable)
            {
                _logger.LogInformation($"Update available: {info.LatestVersion}");
                // Handle update...
            }
        }
        catch (AppUpdateException ex)
        {
            _logger.LogError($"Update error: {ex.Message}");
        }
    }
}
```

## Best Practices

1. **Mock Configuration**
   - Keep mock behavior configurable
   - Simulate realistic scenarios
   - Match production timing

2. **Testing**
   - Test all update flows
   - Include error cases
   - Verify UI states

3. **Integration**
   - Use dependency injection
   - Log mock actions
   - Document mock behavior