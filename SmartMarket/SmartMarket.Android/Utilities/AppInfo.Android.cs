using System;
using System.Globalization;
using Android.Content;
using Android.Content.PM;
using FotoScan.Tablet.Interfaces;
using SmartMarket.Droid.Utilities;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly:Dependency(typeof(AppInfo))]
namespace SmartMarket.Droid.Utilities
{
    public class AppInfo : IAppInfo
    {
        #region PackageName

        public string PackageName => PlatformGetPackageName();

        private string PlatformGetPackageName() => Application.Context.PackageName;

        #endregion

        #region Name

        public string Name => PlatformGetName();
        private string PlatformGetName()
        {
            var applicationInfo = Application.Context.ApplicationInfo;
            var packageManager = Application.Context.PackageManager;
            return applicationInfo.LoadLabel(packageManager);
        }


        #endregion

        #region VersionString

        public string VersionString => PlatformGetVersionString();
        private string PlatformGetVersionString()
        {
            var pm = Application.Context.PackageManager;
            var packageName = Application.Context.PackageName;
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
            {
                return info.VersionName;
            }
        }

        #endregion

        #region Version

        public Version Version => ParseVersion(VersionString);
        private Version ParseVersion(string version)
        {
            if (Version.TryParse(version, out var number))
                return number;

            if (int.TryParse(version, out var major))
                return new Version(major, 0);

            return new Version(0, 0);
        }

        #endregion

        #region VersionCode

        public string VersionCode => PlatformGetBuild();
        private string PlatformGetBuild()
        {
            var pm = Application.Context.PackageManager;
            var packageName = Application.Context.PackageName;
            using (var info = pm.GetPackageInfo(packageName, PackageInfoFlags.MetaData))
            {
                return info.VersionCode.ToString(CultureInfo.InvariantCulture);
            }
        }

        public void ShowSettingsUI()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ShowSettingsUI

        //public void ShowSettingsUI() => PlatformShowSettingsUI();
        //private void PlatformShowSettingsUI()
        //{
        //    var context = XamForms.Context;

        //    var settingsIntent = new Intent();
        //    settingsIntent.SetAction(global::Android.Provider.Settings.ActionApplicationDetailsSettings);
        //    settingsIntent.AddCategory(Intent.CategoryDefault);
        //    settingsIntent.SetData(global::Android.Net.Uri.Parse("package:" + PlatformGetPackageName()));
        //    settingsIntent.AddFlags(ActivityFlags.NewTask);
        //    settingsIntent.AddFlags(ActivityFlags.NoHistory);
        //    settingsIntent.AddFlags(ActivityFlags.ExcludeFromRecents);
        //    context.StartActivity(settingsIntent);
        //}

        #endregion
    }
}