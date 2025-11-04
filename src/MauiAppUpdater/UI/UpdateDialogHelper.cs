#if __IOS__
using UIKit;

namespace MauiAppUpdater.UI
{
    /// <summary>
    /// Helper class for update-related UI components
    /// </summary>
    public static class UpdateDialogHelper
    {
        public static async Task<bool> ShowUpdateDialog(
            UIViewController controller,
            string title,
            string message,
            string updateButtonText = "Update Now",
            string cancelButtonText = "Later")
        {
            var tcs = new TaskCompletionSource<bool>();

            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            
            alert.AddAction(UIAlertAction.Create(updateButtonText, UIAlertActionStyle.Default, 
                action => tcs.SetResult(true)));
            
            alert.AddAction(UIAlertAction.Create(cancelButtonText, UIAlertActionStyle.Cancel, 
                action => tcs.SetResult(false)));

            controller.PresentViewController(alert, true, null);
            return await tcs.Task;
        }
    }
}
#endif