using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Interfaces;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
	public class ProfileUserViewModel : ViewModelBase
    {
        #region constructor
        public ProfileUserViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest, IAppInfo appInfo)
        : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest, appInfo: appInfo)
        {
            LoginSignupCommand = new DelegateCommand(NavigateToLoginPage);
            IsLogin = App.Settings.IsLogin;
        }

        #endregion

        #region Properties
        private bool _isLogin;

        public bool IsLogin
        {
            get { return _isLogin; }
            set { SetProperty(ref _isLogin, value); }
        }
        #endregion

        #region LoginSignupCommand
        public ICommand LoginSignupCommand { get; set; }
        private async void NavigateToLoginPage()
        {
            await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
        }
        #endregion
    }
}
