using System.Threading.Tasks;

namespace MauiAppUpdater
{
    public interface IAppUpdater
    {
        Task<UpdateInfo> CheckForUpdateAsync();
        Task<bool> StartFlexibleUpdateAsync();
        Task<bool> StartImmediateUpdateAsync();
        Task<bool> PromptToOpenStoreAsync();
    }

    public record UpdateInfo(
        bool IsUpdateAvailable,
        string LatestVersion,
        UpdateType Type,
        int Priority,
        string? ReleaseNotes = null
    );

    public enum UpdateType
    {
        None,
        Flexible,
        Immediate
    }

    public enum UpdateStrategy
    {
        Manual,
        Automatic
    }

    public class AppUpdaterOptions
    {
        public string? PlayStorePackageName { get; set; }
        public string? AppStoreIdOrUrl { get; set; }
        public UpdateStrategy UpdateStrategy { get; set; } = UpdateStrategy.Manual;
        public bool ForceUpdate { get; set; }
        public TimeSpan CheckInterval { get; set; } = TimeSpan.FromHours(12);
        public string? LogoImageResource { get; set; }
        public string? PrimaryColor { get; set; }
        public bool EnableDebugLogging { get; set; }
    }
}