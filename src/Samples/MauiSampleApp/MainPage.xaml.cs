using MauiAppUpdater;

namespace MauiSampleApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IAppUpdater _appUpdater;

        public MainPage(IAppUpdater appUpdater)
        {
            InitializeComponent();
            _appUpdater = appUpdater;
        }

        private async void OnCheckUpdateClicked(object sender, EventArgs e)
        {
            try
            {
                StatusLabel.Text = "Checking for updates...";
                var updateInfo = await _appUpdater.CheckForUpdateAsync();

                if (updateInfo.IsUpdateAvailable)
                {
                    bool result = await DisplayAlert(
                        "Update Available",
                        $"Version {updateInfo.LatestVersion} is available.\n{updateInfo.ReleaseNotes}",
                        "Update Now",
                        "Later");

                    if (result)
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
                    else
                    {
                        RemindMeService.SetRemindLater("app_update", 1);
                    }
                }
                else
                {
                    await DisplayAlert("Up to Date", "You have the latest version", "OK");
                }

                StatusLabel.Text = "";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Error: {ex.Message}";
            }
        }
    }
}