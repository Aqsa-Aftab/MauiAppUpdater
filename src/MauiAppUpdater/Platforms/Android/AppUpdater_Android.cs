#if __ANDROID__
using Android.App;
using Android.Content;
using Microsoft.Extensions.Logging;
// Removed Play Core dependencies (Google in-app updates) to allow compilation without external packages.

namespace MauiAppUpdater
{
    #region Strategy
    internal interface IAndroidUpdateStrategy
    {
        Task<UpdateInfo> CheckForUpdateAsync(AppUpdaterOptions options, ILogger? logger);
        Task<bool> StartFlexibleUpdateAsync(AppUpdaterOptions options, Activity currentActivity, ILogger? logger);
        Task<bool> StartImmediateUpdateAsync(AppUpdaterOptions options, Activity currentActivity, ILogger? logger);
    }

    internal sealed class FallbackAndroidUpdateStrategy : IAndroidUpdateStrategy
    {
        public Task<UpdateInfo> CheckForUpdateAsync(AppUpdaterOptions options, ILogger? logger)
            => Task.FromResult(new UpdateInfo(false, "", UpdateType.None, 0));

        public Task<bool> StartFlexibleUpdateAsync(AppUpdaterOptions options, Activity currentActivity, ILogger? logger)
            => OpenStoreAsync(options, currentActivity, logger);

        public Task<bool> StartImmediateUpdateAsync(AppUpdaterOptions options, Activity currentActivity, ILogger? logger)
            => OpenStoreAsync(options, currentActivity, logger);

        private static Task<bool> OpenStoreAsync(AppUpdaterOptions opts, Activity activity, ILogger? logger)
        {
            if (!AppUpdaterValidator.IsValidPlayPackageName(opts.PlayStorePackageName ?? string.Empty))
                throw new AppUpdateException("Invalid PlayStore package name format");

            try
            {
                var intent = new Intent(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse($"market://details?id={opts.PlayStorePackageName}"));
                activity.StartActivity(intent);
                logger?.LogInformation("Opened Play Store for package {Package}", opts.PlayStorePackageName);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Play Store app not available, trying web URL.");
                try
                {
                    var webIntent = new Intent(Intent.ActionView);
                    webIntent.SetData(Android.Net.Uri.Parse($"https://play.google.com/store/apps/details?id={opts.PlayStorePackageName}"));
                    activity.StartActivity(webIntent);
                    logger?.LogInformation("Opened Play Store web URL for package {Package}", opts.PlayStorePackageName);
                    return Task.FromResult(true);
                }
                catch
                {
                    throw new AppUpdateException("Failed to open Play Store", ex);
                }
            }
        }
    }
    #endregion

    public class AppUpdater_Android : IAppUpdater
    {
        private readonly AppUpdaterOptions _options;
        private readonly ILogger? _logger;
        private Activity? CurrentActivity => Platform.CurrentActivity;
        private readonly IAndroidUpdateStrategy _strategy;

    public AppUpdater_Android(AppUpdaterOptions options, ILogger? logger = null)
        {
            _options = options;
            _logger = logger;
            PlatformUtils.ValidateConfiguration(options);
            _strategy = new FallbackAndroidUpdateStrategy();
        }

        public Task<UpdateInfo> CheckForUpdateAsync() => _strategy.CheckForUpdateAsync(_options, _logger);

        public Task<bool> StartFlexibleUpdateAsync()
        {
            if (CurrentActivity == null)
                throw new AppUpdateException("No activity available");
            _logger?.LogInformation("Starting flexible update flow.");
            return _strategy.StartFlexibleUpdateAsync(_options, CurrentActivity, _logger);
        }

        public Task<bool> StartImmediateUpdateAsync()
        {
            if (CurrentActivity == null)
                throw new AppUpdateException("No activity available");
            _logger?.LogInformation("Starting immediate update flow.");
            return _strategy.StartImmediateUpdateAsync(_options, CurrentActivity, _logger);
        }

        public Task<bool> PromptToOpenStoreAsync()
        {
            if (CurrentActivity == null)
                throw new AppUpdateException("No activity available");
            return _strategy.StartFlexibleUpdateAsync(_options, CurrentActivity, _logger);
        }
    }
}
#endif