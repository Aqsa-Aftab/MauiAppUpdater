#if __ANDROID__
using Android.App;
using Android.Content;
// Removed Play Core dependencies (Google in-app updates) to allow compilation without external packages.

namespace MauiAppUpdater
{
    public class AppUpdater_Android : IAppUpdater
    {
        private readonly AppUpdaterOptions _options;
    private Activity? CurrentActivity => Platform.CurrentActivity;

        public AppUpdater_Android(AppUpdaterOptions options)
        {
            _options = options;
            PlatformUtils.ValidateConfiguration(options);
        }

        public async Task<UpdateInfo> CheckForUpdateAsync()
        {
            // Stub implementation: Without Play Core bindings, we can't query update availability.
            // Returns no update. Future enhancement: add proper Play Core package and logic.
            return await Task.FromResult(new UpdateInfo(false, "", UpdateType.None, 0));
        }

        public async Task<bool> StartFlexibleUpdateAsync()
        {
            // Without Play Core, fallback: open Play Store page.
            return await PromptToOpenStoreAsync();
        }

        public async Task<bool> StartImmediateUpdateAsync()
        {
            // Fallback behavior identical to flexible update.
            return await PromptToOpenStoreAsync();
        }

        public async Task<bool> PromptToOpenStoreAsync()
        {
            if (CurrentActivity == null)
                throw new AppUpdateException("No activity available");

            try
            {
                var intent = new Intent(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse($"market://details?id={_options.PlayStorePackageName}"));
                CurrentActivity.StartActivity(intent);
                return true;
            }
            catch (Exception ex)
            {
                // If Play Store app is not installed, try web browser
                try
                {
                    var webIntent = new Intent(Intent.ActionView);
                    webIntent.SetData(Android.Net.Uri.Parse(
                        $"https://play.google.com/store/apps/details?id={_options.PlayStorePackageName}"));
                    CurrentActivity.StartActivity(webIntent);
                    return true;
                }
                catch
                {
                    throw new AppUpdateException("Failed to open Play Store", ex);
                }
            }
        }
    }
}
#endif