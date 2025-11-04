using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MauiAppUpdater
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder AddMauiAppUpdater(
            this MauiAppBuilder builder,
            Action<AppUpdaterOptions> configure)
        {
            var options = new AppUpdaterOptions();
            configure(options);

            builder.Services.AddSingleton(options);

            builder.Services.TryAddSingleton<IAppUpdater>(services =>
            {
                #if __ANDROID__
                return new AppUpdater_Android(options);
                #elif __IOS__
                return new AppUpdater_iOS(options);
                #else
                throw new PlatformNotSupportedException("The current platform is not supported.");
                #endif
            });

            return builder;
        }
    }
}