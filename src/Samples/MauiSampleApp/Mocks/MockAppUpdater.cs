using MauiAppUpdater;

namespace MauiSampleApp
{
    public class MockAppUpdater : IAppUpdater
    {
        public Task<UpdateInfo> CheckForUpdateAsync()
        {
            return Task.FromResult(new UpdateInfo(
                IsUpdateAvailable: true,
                LatestVersion: "2.0.0",
                Type: UpdateType.Flexible,
                Priority: 1,
                ReleaseNotes: "Test update available"
            ));
        }

        public Task<bool> StartFlexibleUpdateAsync() => Task.FromResult(true);
        public Task<bool> StartImmediateUpdateAsync() => Task.FromResult(true);
        public Task<bool> PromptToOpenStoreAsync() => Task.FromResult(true);
    }
}