using System.Text.RegularExpressions;

namespace MauiAppUpdater
{
    public static class AppUpdaterValidator
    {
        public static bool IsValidPlayPackageName(string packageName)
        {
            if (string.IsNullOrWhiteSpace(packageName))
                return false;

            // Require at least three segments (e.g., com.company.app) and each segment >= 2 chars except first which must be >=3 (to avoid 'com.example' false positive for app name missing)
            // Adjust rule: first segment >=3, total segments >=3, last segment >=2.
            var parts = packageName.Split('.');
            if (parts.Length < 3) return false;
            if (parts[0].Length < 3) return false;
            if (parts[^1].Length < 2) return false;
            // Validate each part with regex.
            var partRegex = new Regex(@"^[a-z][a-z0-9_]*$", RegexOptions.Compiled);
            foreach (var p in parts)
            {
                if (!partRegex.IsMatch(p)) return false;
            }
            return true;
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