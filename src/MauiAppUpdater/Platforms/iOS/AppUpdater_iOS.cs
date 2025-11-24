#if __IOS__
using Foundation;
using UIKit;
using System.Threading.Tasks;

namespace MauiAppUpdater
{
    public class AppUpdater_iOS : IAppUpdater
    {
        private readonly AppUpdaterOptions _options;
        private UIViewController? CurrentController => GetCurrentController();

        public AppUpdater_iOS(AppUpdaterOptions options)
        {
            _options = options;
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
                string urlString = _options.AppStoreIdOrUrl;
                if (!urlString.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    urlString = $"itms-apps://apps.apple.com/app/id{urlString}";
                }

                var nsUrl = new NSUrl(urlString);
                if (!UIApplication.SharedApplication.CanOpenUrl(nsUrl))
                    return false;

                var tcs = new TaskCompletionSource<bool>();
                // Modern OpenUrl overload with completion handler (bridged if available).
                if (UIApplication.SharedApplication.RespondsToSelector(new ObjCRuntime.Selector("openURL:options:completionHandler:")))
                {
                    UIApplication.SharedApplication.OpenUrl(nsUrl, new NSDictionary(), (success) =>
                    {
                        tcs.TrySetResult(success);
                    });
                    return await tcs.Task.ConfigureAwait(false);
                }
                // Fallback to deprecated synchronous call.
                bool opened = UIApplication.SharedApplication.OpenUrl(nsUrl);
                return opened;
            }
            catch (Exception ex)
            {
                throw new AppUpdateException("Failed to open App Store", ex);
            }
        }

        private UIViewController? GetCurrentController()
        {
            // Multi-scene safe approach.
            var windowScene = UIApplication.SharedApplication.ConnectedScenes
                .OfType<UIWindowScene>()
                .FirstOrDefault();
            var window = windowScene?.Windows.FirstOrDefault(w => w.IsKeyWindow);
            var rootController = window?.RootViewController;
            var current = rootController;
            while (current?.PresentedViewController != null)
                current = current.PresentedViewController;
            return current;
        }
    }
}
#endif