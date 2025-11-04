using System.Text.RegularExpressions;

namespace MauiAppUpdater
{
    public static class AppUpdaterValidator
    {
        public static bool IsValidPlayPackageName(string packageName)
        {
            if (string.IsNullOrWhiteSpace(packageName))
                return false;

            // Play Store package names must be in reverse domain name format
            // e.g., com.company.app
            var regex = new Regex(@"^[a-z][a-z0-9_]*(\.[a-z][a-z0-9_]*)+$");
            return regex.IsMatch(packageName);
        }

        public static bool IsValidAppleIdOrUrl(string idOrUrl)
        {
            if (string.IsNullOrWhiteSpace(idOrUrl))
                return false;

            // Allow numeric App Store IDs
            if (long.TryParse(idOrUrl, out _))
                return true;

            // Allow App Store URLs
            var urlRegex = new Regex(@"^https://apps\.apple\.com/.*/id\d+");
            return urlRegex.IsMatch(idOrUrl);
        }
    }
}