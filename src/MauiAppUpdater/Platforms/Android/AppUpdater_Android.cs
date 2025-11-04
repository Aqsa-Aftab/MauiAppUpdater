#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Com.Google.Android.Play.Core.Appupdate;
using Com.Google.Android.Play.Core.Install.Model;

namespace MauiAppUpdater
{
    public class AppUpdater_Android : IAppUpdater
    {
        private readonly AppUpdaterOptions _options;
        private readonly IAppUpdateManager _updateManager;
        private Activity? CurrentActivity => Platform.CurrentActivity;
        private const int UPDATE_REQUEST_CODE = 500;

        public AppUpdater_Android(AppUpdaterOptions options)
        {
            _options = options;
            _updateManager = AppUpdateManagerFactory.Create(Application.Context);
            PlatformUtils.ValidateConfiguration(options);
        }

        public async Task<UpdateInfo> CheckForUpdateAsync()
        {
            try
            {
                // Get app update info
                var appUpdateInfo = await _updateManager.AppUpdateInfo
                    .AsAsync<Com.Google.Android.Play.Core.Appupdate.AppUpdateInfo>();

                if (appUpdateInfo.UpdateAvailability() == UpdateAvailability.UpdateAvailable)
                {
                    bool canFlex = appUpdateInfo.IsUpdateTypeAllowed(AppUpdateType.Flexible);
                    bool canImmediate = appUpdateInfo.IsUpdateTypeAllowed(AppUpdateType.Immediate);
                    
                    if (!canFlex && !canImmediate)
                        return new UpdateInfo(false, "", UpdateType.None, 0);

                    return new UpdateInfo(
                        IsUpdateAvailable: true,
                        LatestVersion: appUpdateInfo.AvailableVersionCode().ToString(),
                        Type: _options.ForceUpdate || !canFlex ? UpdateType.Immediate : UpdateType.Flexible,
                        Priority: appUpdateInfo.UpdatePriority(),
                        ReleaseNotes: null // Play Core doesn't provide notes
                    );
                }

                return new UpdateInfo(false, "", UpdateType.None, 0);
            }
            catch (Exception ex)
            {
                throw new AppUpdateException("Failed to check for updates", ex);
            }
        }

        public async Task<bool> StartFlexibleUpdateAsync()
        {
            if (CurrentActivity == null)
                throw new AppUpdateException("No activity available");

            try
            {
                var appUpdateInfo = await _updateManager.AppUpdateInfo
                    .AsAsync<Com.Google.Android.Play.Core.Appupdate.AppUpdateInfo>();
                
                if (appUpdateInfo.UpdateAvailability() != UpdateAvailability.UpdateAvailable ||
                    !appUpdateInfo.IsUpdateTypeAllowed(AppUpdateType.Flexible))
                    return false;

                var task = _updateManager.StartUpdateFlowForResult(
                    appUpdateInfo,
                    AppUpdateType.Flexible,
                    CurrentActivity,
                    UPDATE_REQUEST_CODE);

                // Handle result via Activity.OnActivityResult
                return task.IsSuccessful;
            }
            catch (Exception ex)
            {
                throw new AppUpdateException("Failed to start flexible update", ex);
            }
        }

        public async Task<bool> StartImmediateUpdateAsync()
        {
            if (CurrentActivity == null)
                throw new AppUpdateException("No activity available");

            try
            {
                var appUpdateInfo = await _updateManager.AppUpdateInfo
                    .AsAsync<Com.Google.Android.Play.Core.Appupdate.AppUpdateInfo>();
                
                if (appUpdateInfo.UpdateAvailability() != UpdateAvailability.UpdateAvailable ||
                    !appUpdateInfo.IsUpdateTypeAllowed(AppUpdateType.Immediate))
                    return false;

                var task = _updateManager.StartUpdateFlowForResult(
                    appUpdateInfo,
                    AppUpdateType.Immediate,
                    CurrentActivity,
                    UPDATE_REQUEST_CODE);

                return task.IsSuccessful;
            }
            catch (Exception ex)
            {
                throw new AppUpdateException("Failed to start immediate update", ex);
            }
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