using Microsoft.Maui.Storage;

namespace MauiAppUpdater
{
    public static class RemindMeService
    {
        private const string PreferenceKeyPrefix = "maui_appupdater_remind_";
        
        public static bool ShouldPrompt(string key)
        {
            var prefKey = GetPreferenceKey(key);
            var value = Preferences.Get(prefKey, 0L);
            return value == 0 || DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= value;
        }

        public static void SetRemindLater(string key, int daysLater)
        {
            var prefKey = GetPreferenceKey(key);
            var remindTime = DateTimeOffset.UtcNow.AddDays(daysLater).ToUnixTimeSeconds();
            Preferences.Set(prefKey, remindTime);
        }

        public static void Clear(string key)
        {
            var prefKey = GetPreferenceKey(key);
            Preferences.Remove(prefKey);
        }

        private static string GetPreferenceKey(string key) => PreferenceKeyPrefix + key;
    }
}