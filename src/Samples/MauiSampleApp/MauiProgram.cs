namespace MauiSampleApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            builder.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddMauiAppUpdater(options =>
            {
                #if __ANDROID__
                options.PlayStorePackageName = "com.example.mauisample";
                #elif __IOS__
                options.AppStoreIdOrUrl = "1234567890";
                #endif
                
                options.UpdateStrategy = UpdateStrategy.Automatic;
                options.ForceUpdate = false;
                options.CheckInterval = TimeSpan.FromHours(12);
                options.EnableDebugLogging = true;
            });

            #if DEBUG
            builder.Services.AddSingleton<IAppUpdater, MockAppUpdater>();
            #endif

            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}