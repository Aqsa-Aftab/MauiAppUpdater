#if __ANDROID__
using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Content;

namespace MauiSampleApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private const int UPDATE_REQUEST_CODE = 500;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == UPDATE_REQUEST_CODE)
            {
                if (resultCode != Result.Ok)
                {
                    System.Diagnostics.Debug.WriteLine("Update flow failed! Result code: " + resultCode);
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
#endif