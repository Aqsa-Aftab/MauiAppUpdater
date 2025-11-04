namespace MauiAppUpdater
{
    /// <summary>
    /// Extension methods for platform-specific utilities
    /// </summary>
    internal static class PlatformUtils
    {
        /// <summary>
        /// Parses version strings into comparable Version objects
        /// </summary>
        public static Version ParseVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                throw new AppUpdateException("Invalid version string");

            // Remove any non-numeric prefixes (like 'v' or 'version')
            version = System.Text.RegularExpressions.Regex.Replace(version, @"[^\d.].*$", "");
            
            if (!Version.TryParse(version, out var parsedVersion))
                throw new AppUpdateException($"Could not parse version: {version}");

            return parsedVersion;
        }

        /// <summary>
        /// Compares two version strings
        /// </summary>
        public static bool IsNewerVersion(string current, string latest)
        {
            var currentVersion = ParseVersion(current);
            var latestVersion = ParseVersion(latest);
            
            return latestVersion > currentVersion;
        }

        /// <summary>
        /// Validates update configuration
        /// </summary>
        public static void ValidateConfiguration(AppUpdaterOptions options)
        {
            #if __ANDROID__
            if (string.IsNullOrWhiteSpace(options.PlayStorePackageName))
                throw new AppUpdateException("PlayStore package name not configured");
            
            if (!AppUpdaterValidator.IsValidPlayPackageName(options.PlayStorePackageName))
                throw new AppUpdateException("Invalid PlayStore package name format");
            #elif __IOS__
            if (string.IsNullOrWhiteSpace(options.AppStoreIdOrUrl))
                throw new AppUpdateException("App Store ID/URL not configured");
            
            if (!AppUpdaterValidator.IsValidAppleIdOrUrl(options.AppStoreIdOrUrl))
                throw new AppUpdateException("Invalid App Store ID/URL format");
            #endif
        }
    }
}