using Xunit;
using MauiAppUpdater;
namespace MauiAppUpdater.Tests
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData("com.example.app", true)]
        [InlineData("com.example", false)]
        public void PlayPackageValidation(string input, bool expected) { Assert.Equal(expected, AppUpdaterValidator.IsValidPlayPackageName(input)); }

        [Theory]
        [InlineData("123456789", true)]
        [InlineData("https://apps.apple.com/us/app/foo/id123456789", true)]
        public void AppleValidation(string input, bool expected) { Assert.Equal(expected, AppUpdaterValidator.IsValidAppleIdOrUrl(input)); }
    }
}
