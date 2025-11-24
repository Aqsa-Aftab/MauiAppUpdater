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

        public async Task<UpdateInfo> CheckForUpdateAsync()
        {
            if (string.IsNullOrEmpty(_options.AppStoreIdOrUrl))
                throw new AppUpdateException("App Store ID/URL not configured");

            try
            {
                // Note: In actual implementation, this would check iTunes API
                // This is a mock implementation
                return new UpdateInfo(
                    IsUpdateAvailable: true,
                    LatestVersion: "2.0.0",
                    Type: _options.ForceUpdate ? UpdateType.Immediate : UpdateType.Flexible,
                    Priority: 1,
                    ReleaseNotes: "New version available"
                );
            }
            catch (Exception ex)
            {
                throw new AppUpdateException("Failed to check for updates", ex);
            }
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
                if (UIApplication.SharedApplication.CanOpenUrl(nsUrl))
                {
                    // Synchronous open (iOS API); wrap in Task for API consistency.
                    UIApplication.SharedApplication.OpenUrl(nsUrl);
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                throw new AppUpdateException("Failed to open App Store", ex);
            }
        }

        private UIViewController? GetCurrentController()
        {
            UIWindow? window = UIApplication.SharedApplication.KeyWindow;
            UIViewController? rootController = window?.RootViewController;
            
            UIViewController? current = rootController;
            while (current?.PresentedViewController != null)
            {
                current = current.PresentedViewController;
            }
            
            return current;
        }
    }
}
#endif