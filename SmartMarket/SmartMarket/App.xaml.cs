using Prism;
using Prism.Ioc;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
using SmartMarket.Services.HttpService;
using SmartMarket.Services.SQLiteService;
using SmartMarket.Utilities;
using SmartMarket.ViewModels;
using SmartMarket.Views;
using SmartMarket.Views.LoginAndSignUp;
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

            await NavigationService.NavigateAsync($"{PageManager.TabbedMainPage}");

        }

        #endregion

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
        }
    }
}
