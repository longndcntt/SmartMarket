using Foundation;
using SmartMarket.Interfaces;
using SmartMarket.iOS.Utilities;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppInfo))]
namespace SmartMarket.iOS.Utilities
{
    public class AppInfo : IAppInfo
    {
        #region PackageName

        public string PackageName => PlatformGetPackageName();

        private string PlatformGetPackageName() => GetBundleValue("CFBundleIdentifier");

        #endregion

        #region Name

        public string Name => PlatformGetName();
        private string PlatformGetName() => GetBundleValue("CFBundleDisplayName") ?? GetBundleValue("CFBundleName");

        #endregion

        #region VersionString

        public string VersionString => PlatformGetVersionString();
        private string PlatformGetVersionString() => GetBundleValue("CFBundleShortVersionString");

        #endregion

        #region Version

        

        #endregion

        #region VersionCode

        public string VersionCode => PlatformGetBuild();
        private string PlatformGetBuild() => GetBundleValue("CFBundleVersion");

        #endregion

        #region GetBundleValue

        private string GetBundleValue(string key)
           => NSBundle.MainBundle.ObjectForInfoDictionary(key)?.ToString();

        #endregion

        #region ShowSettingsUI

        public void ShowSettingsUI() => PlatformShowSettingsUI();
        private void PlatformShowSettingsUI() =>
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));

        #endregion
    }
}