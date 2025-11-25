using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

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
                var loggerFactory = services.GetService<ILoggerFactory>();
                #if __ANDROID__
                var androidLogger = loggerFactory?.CreateLogger<AppUpdater_Android>();
                return new AppUpdater_Android(options, androidLogger);
                #elif __IOS__
                var iosLogger = loggerFactory?.CreateLogger<AppUpdater_iOS>();
                return new AppUpdater_iOS(options, iosLogger);
                #else
                throw new PlatformNotSupportedException("The current platform is not supported.");
                #endif
            });

            return builder;
        }
    }
}