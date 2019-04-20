using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Platform;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using SmartMarket.Droid.Services.SQLiteService;
using SmartMarket.Droid.Utilities;
using SmartMarket.Interfaces;
using SmartMarket.Interfaces.LocalDatabase;

namespace SmartMarket.Droid
{
    [Activity(Label = "SmartMarket", 
        Icon = "@mipmap/ic_launcher", 
        Theme = "@style/MainTheme", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.User, WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Init(bundle);


            LoadApplication(new App(new AndroidInitializer()));
        }

        #region Init

        public void Init(Bundle bundle)
        {
            RequestPermission();
            CarouselViewRenderer.Init();
            CachedImageRenderer.Init(true);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
          
            InitScreen();
            Xamarin.Forms.Forms.Init(this, bundle);
        }

        #endregion

        #region InitScreen

        private void InitScreen()
        {
            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;
        }

        #endregion

        #region Permissions
        /// <summary>
        /// Permission request
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode,
            string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void RequestPermission()
        {
            await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Storage);
        }

        #endregion
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterInstance<IAppInfo>(new AppInfo());
            containerRegistry.RegisterInstance<IDatabaseConnection>(new DatabaseConnection());
        }
    }
}

