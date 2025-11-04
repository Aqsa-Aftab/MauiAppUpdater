namespace MauiAppUpdater
{
    public class AppUpdateException : Exception
    {
        public AppUpdateException(string message) : base(message)
        {
        }

        public AppUpdateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}