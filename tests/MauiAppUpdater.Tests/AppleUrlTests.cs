using Xunit;
using MauiAppUpdater;

namespace MauiAppUpdater.Tests
{
    public class AppleUrlTests
    {
        [Theory]
        [InlineData("1234567890", "itms-apps://apps.apple.com/app/id1234567890")]
        [InlineData("https://apps.apple.com/us/app/foo/id1234567890", "itms-apps://apps.apple.com/app/id1234567890")]
        public void BuildSafeAppleStoreUrl_ValidInputs(string input, string expected)
        {
            var actual = PlatformUtils.BuildSafeAppleStoreUrl(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("not-a-url")]
        [InlineData("https://malicious.example.com/id123")]        
        public void BuildSafeAppleStoreUrl_InvalidInputs(string input)
        {
            Assert.Throws<AppUpdateException>(() => PlatformUtils.BuildSafeAppleStoreUrl(input));
        }
    }
}
