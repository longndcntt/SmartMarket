using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.Services.HttpService;
using SmartMarket.Services.SQLiteService;
using SmartMarket.Utilities;
using SmartMarket.ViewModels;
using SmartMarket.Views;
using SmartMarket.Views.LoginAndSignUp;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SmartMarket
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitDatabase();
            InitializeComponent();

            StartApp();
        }

        #region Properties 

        public new static App Current => Application.Current as App;
        public static double ScreenWidth;
        public static double ScreenHeight;

        public static bool IsBusy = true;

        private ISqLiteService _sqLiteService;
        public static AppSettings Settings { get; set; }

        #endregion

        #region InitDatabase

        private void InitDatabase()
        {
            var connectionService = DependencyService.Get<IDatabaseConnection>();
            _sqLiteService = new SqLiteService(connectionService);
        }

        #endregion

        #region StartApp

        private async void StartApp()
        {
            Settings = new AppSettings();
            Settings = _sqLiteService.GetSettings();
            Settings.HttpUrl1 = "http://35.194.122.245:3000/";
            await NavigationService.NavigateAsync($"{PageManager.TabbedMainPage}");

        }

        #endregion

        protected override void OnStart()
        {
            base.OnStart();
            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            };
            System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    var noti = new NotificationModel();
                    if (p.Data.ContainsKey("body"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            p.Data["title"] = TranslateExtension.Get(p.Data["TitleKey"].ToString());
                            noti.Title = p.Data["title"].ToString();
                            if (p.Data.ContainsKey("Coin"))
                            {
                                p.Data["body"] = string.Format(TranslateExtension.Get(p.Data["MessageKey"].ToString()), p.Data["Coin"].ToString());
                                noti.Message = p.Data["body"].ToString();
                            }
                            else
                            {
                                p.Data["body"] = TranslateExtension.Get(p.Data["MessageKey"].ToString());
                                noti.Message = p.Data["body"].ToString();
                            }
                            //noti.Title = p.Data["title"].ToString();
                            noti.DateTimeReceived = p.Data["DateTimeSend"].ToString();
                            _sqLiteService.Insert(noti);
                        });

                    }
                }
                catch (Exception ex)
                {

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //System.Diagnostics.Debug.WriteLine(p.Identifier);

                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
                if (p.Data.ContainsKey("aps.alert.title"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        System.Diagnostics.Debug.WriteLine($"{p.Data["aps.alert.title"]}");
                    });

                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<TabbedMainPage, TabbedMainPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUp, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<ProfileUser, ProfileUserViewModel>();
            containerRegistry.RegisterForNavigation<NotificationPage, NotificationPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginSignUpTabbedPage, LoginSignUpTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfileUserPage, ProfileUserPageViewModel>();
            containerRegistry.RegisterForNavigation<CategoryPage, CategoryPageViewModel>();
            containerRegistry.RegisterForNavigation<CategoryDetailsPage, CategoryDetailsPageViewModel>();

            containerRegistry.Register<IHttpRequest, HttpRequest>();
            containerRegistry.Register<ISqLiteService, SqLiteService>();
            containerRegistry.RegisterForNavigation<ItemDetailsPage, ItemDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowCardPage, ShowCardPageViewModel>();
            containerRegistry.RegisterForNavigation<ProceedToCheckoutPage, ProceedToCheckoutPageViewModel>();
            containerRegistry.RegisterForNavigation<WalletBalancePage, WalletBalancePageViewModel>();
            containerRegistry.RegisterForNavigation<AddMoneyPage, AddMoneyPageViewModel>();
            containerRegistry.RegisterForNavigation<MessagePage, MessagePageViewModel>();
            containerRegistry.RegisterForNavigation<SearchItemPage, SearchItemPageViewModel>();
            containerRegistry.RegisterForNavigation<UploadProductPage, UploadProductPageViewModel>();
            containerRegistry.RegisterForNavigation<PurchaseedProduct, PurchaseedProductViewModel>();
        }
    }
}
