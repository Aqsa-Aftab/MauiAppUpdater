#if __IOS__
using Foundation;
using UIKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MauiAppUpdater
{
    public class AppUpdater_iOS : IAppUpdater
    {
        private readonly AppUpdaterOptions _options;
    private readonly ILogger? _logger;
        private UIViewController? CurrentController => GetCurrentController();

    public AppUpdater_iOS(AppUpdaterOptions options, ILogger? logger = null)
        {
            _options = options;
            _logger = logger;
        }

        public Task<UpdateInfo> CheckForUpdateAsync()
        {
            if (string.IsNullOrEmpty(_options.AppStoreIdOrUrl))
                throw new AppUpdateException("App Store ID/URL not configured");

            // Mock implementation; replace with iTunes API query later.
            var info = new UpdateInfo(
                IsUpdateAvailable: true,
                LatestVersion: "2.0.0",
                Type: _options.ForceUpdate ? UpdateType.Immediate : UpdateType.Flexible,
                Priority: 1,
                ReleaseNotes: "New version available"
            );
            return Task.FromResult(info);
        }

        public async Task<bool> StartFlexibleUpdateAsync()
        {
            // On iOS, flexible and immediate updates both redirect to App Store
            return await PromptToOpenStoreAsync();
        }

        public async Task<bool> StartImmediateUpdateAsync()
        {
            return await PromptToOpenStoreAsync();
        }

        public async Task<bool> PromptToOpenStoreAsync()
        {
            if (CurrentController == null)
                throw new AppUpdateException("No view controller available");

            try
            {
                var idOrUrl = _options.AppStoreIdOrUrl ?? throw new AppUpdateException("App Store ID/URL not configured");
                var safeUrl = PlatformUtils.BuildSafeAppleStoreUrl(idOrUrl);
                var nsUrl = new NSUrl(safeUrl);
                if (!UIApplication.SharedApplication.CanOpenUrl(nsUrl))
                    return false;

                // Prefer modern completion handler if available; else use newer options overload (not deprecated) then return true.
                var selector = new ObjCRuntime.Selector("openURL:options:completionHandler:");
                if (UIApplication.SharedApplication.RespondsToSelector(selector))
                {
                    var tcs = new TaskCompletionSource<bool>();
                    _logger?.LogInformation("Opening App Store URL {Url}", safeUrl);
                    UIApplication.SharedApplication.OpenUrl(nsUrl, new NSDictionary(), success => tcs.TrySetResult(success));
                    return await tcs.Task.ConfigureAwait(false);
                }
                // Fallback for older API (target platforms should support modern version); return false without calling deprecated method.
                return false;
            }
            catch (Exception ex)
            {
                throw new AppUpdateException("Failed to open App Store", ex);
            }
        }

        private UIViewController? GetCurrentController()
        {
            var windowScene = UIApplication.SharedApplication.ConnectedScenes
                .OfType<UIWindowScene>()
                .FirstOrDefault();
            if (windowScene == null)
                return null;
            var keyWindow = windowScene.Windows.FirstOrDefault(w => w.IsKeyWindow);
            if (keyWindow == null)
                return null;
            var current = keyWindow.RootViewController;
            if (current == null)
                return null;
            while (current.PresentedViewController is not null)
                current = current.PresentedViewController;
            return current;
        }
    }
}
#endif