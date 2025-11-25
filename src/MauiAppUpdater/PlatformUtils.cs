namespace MauiAppUpdater
{
    /// <summary>
    /// Extension methods for platform-specific utilities
    /// </summary>
    public static class PlatformUtils
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

        /// <summary>
        /// Builds a safe App Store URL using the itms-apps scheme from either a numeric ID or a full apps.apple.com URL.
        /// </summary>
        public static string BuildSafeAppleStoreUrl(string idOrUrl)
        {
            if (string.IsNullOrWhiteSpace(idOrUrl))
                throw new AppUpdateException("App Store ID/URL not configured");

            string? id = null;
            // Numeric ID
            if (long.TryParse(idOrUrl, out _))
            {
                id = idOrUrl;
            }
            else
            {
                var m = System.Text.RegularExpressions.Regex.Match(idOrUrl, @"^https://apps\.apple\.com/.*/id(\d+)");
                if (m.Success)
                    id = m.Groups[1].Value;
            }

            if (string.IsNullOrEmpty(id))
                throw new AppUpdateException("Invalid App Store ID/URL format");

            return $"itms-apps://apps.apple.com/app/id{id}";
        }
    }
}