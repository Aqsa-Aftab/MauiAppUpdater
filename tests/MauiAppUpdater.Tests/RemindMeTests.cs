using Xunit;
using MauiAppUpdater;
namespace MauiAppUpdater.Tests
{
    public class RemindTests
    {
        [Fact]
        public void RemindSetClear()
        {
            var key = "test_key_xyz";
            RemindMeService.Clear(key);
            Assert.True(RemindMeService.ShouldPrompt(key));
            RemindMeService.SetRemindLater(key, 1);
            Assert.False(RemindMeService.ShouldPrompt(key));
            RemindMeService.Clear(key);
            Assert.True(RemindMeService.ShouldPrompt(key));
        }
    }
}
