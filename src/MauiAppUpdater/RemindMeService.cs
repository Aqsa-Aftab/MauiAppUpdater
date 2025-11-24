using Microsoft.Maui.Storage;
using System.Collections.Concurrent;
using Microsoft.Maui.ApplicationModel;

namespace MauiAppUpdater
{
    public static class RemindMeService
    {
        private const string PreferenceKeyPrefix = "maui_appupdater_remind_";
    // In-memory fallback when Preferences not implemented (plain net9 target during tests)
    private static readonly ConcurrentDictionary<string, long> InMemoryStore = new();
        
        public static bool ShouldPrompt(string key)
        {
            var prefKey = GetPreferenceKey(key);
            long value;
            if (!TryGetPreference(prefKey, out value))
            {
                value = InMemoryStore.TryGetValue(prefKey, out var stored) ? stored : 0L;
            }
            return value == 0 || DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= value;
        }

        public static void SetRemindLater(string key, int daysLater)
        {
            var prefKey = GetPreferenceKey(key);
            var remindTime = DateTimeOffset.UtcNow.AddDays(daysLater).ToUnixTimeSeconds();
            if (!TrySetPreference(prefKey, remindTime))
            {
                InMemoryStore[prefKey] = remindTime;
            }
        }

        public static void Clear(string key)
        {
            var prefKey = GetPreferenceKey(key);
            if (!TryRemovePreference(prefKey))
            {
                InMemoryStore.TryRemove(prefKey, out _);
            }
        }

        private static string GetPreferenceKey(string key) => PreferenceKeyPrefix + key;

        private static bool TryGetPreference(string key, out long value)
        {
            try
            {
                value = Preferences.Get(key, 0L);
                return true;
            }
            catch (Exception)
            {
                value = 0L;
                return false;
            }
        }

        private static bool TrySetPreference(string key, long value)
        {
            try
            {
                Preferences.Set(key, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TryRemovePreference(string key)
        {
            try
            {
                Preferences.Remove(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}