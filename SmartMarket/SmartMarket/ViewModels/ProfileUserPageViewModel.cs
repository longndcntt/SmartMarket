using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Interfaces;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
	public class ProfileUserPageViewModel : ViewModelBase
	{

        #region Properties
        private bool _isLogin;

        public bool IsLogin
        {
            get { return _isLogin; }
            set { SetProperty(ref _isLogin, value); }
        }

        private string _clientName;

        public string ClientName
        {
            get { return _clientName; }
            set { SetProperty(ref _clientName, value); }
        }
        #endregion

        public ProfileUserPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest, IAppInfo appInfo)
        : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest, appInfo: appInfo)
        {
            LoginSignupCommand = new DelegateCommand(NavigateToLoginPage);
            NavigateWalletBalancePage = new DelegateCommand(NavigateWalletBalancePageExcute);
            // IsLogin = App.Settings.IsLogin;
            IsLogin = App.Settings.IsLogin;
            ClientName = "Người bán hàng 1";
        }

        #region LoginSignupCommand
        public ICommand LoginSignupCommand { get; set; }
        private async void NavigateToLoginPage()
        {
            if (!IsLogin)
            {
                await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
            }
        }
        #endregion

        #region CheckInformationCommand
        public ICommand CheckInformationCommand { get; set; }
        #endregion

        #region NavigateWalletBalancePage
        public ICommand NavigateWalletBalancePage { get; set; }
        private async void NavigateWalletBalancePageExcute()
        {
            await Navigation.NavigateAsync(PageManager.WalletBalancePage);
        }

        #endregion
    }
}
