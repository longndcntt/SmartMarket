using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Enums;
using SmartMarket.Interfaces;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
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

        private bool _isAdmin;

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { SetProperty(ref _isAdmin, value); }
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
            CheckInformationCommand = new DelegateCommand(CheckInformationExcute);
            PurchasedProdcutCommand = new DelegateCommand(PurchasedProdcutExcute);
            ViewedProdcutCommand = new DelegateCommand(ViewedProdcutExcute);
            UploadItemCommand = new DelegateCommand(UploadItemExcute);

            LogOutCommand = new DelegateCommand(LogOutExcute);
            // IsLogin = App.Settings.IsLogin;
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

        #region OnAppear
        public override void OnAppear()
        {
            base.OnAppear();
            IsLogin = App.Settings.IsLogin;
            if (IsLogin)
            {
                UserInfo = SqLiteService.Get<UserModel>(x => x.Id != -1);
                IsAdmin = UserInfo.Email == "admin@admin.com" ? true : false;
                IsExistImage = !string.IsNullOrEmpty(UserInfo.Image);
            }
        }
        #endregion

        #region CheckInformationCommand
        public ICommand CheckInformationCommand { get; set; }
        private async void CheckInformationExcute()
        {
            if (IsLogin)
            {
                var param = new NavigationParameters()
                {
                    {ParamKey.UserInfo.ToString(), UserInfo},
                    {ParamKey.IsEdit.ToString(), true},
                };
                await Navigation.NavigateAsync(PageManager.SignUpPage,param);
            }
            else
            {
                await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
            }
        }
        #endregion

        #region ViewedProdcut
        public ICommand ViewedProdcutCommand { get; set; }
        private async void ViewedProdcutExcute()
        {
            if (IsLogin)
            {
                await Navigation.NavigateAsync(PageManager.WalletBalancePage);
            }
            else
            {
                await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
            }
        }
        #endregion

        #region ViewedProdcut
        public ICommand PurchasedProdcutCommand { get; set; }
        private async void PurchasedProdcutExcute()
        {
            if (IsLogin)
            {
                await Navigation.NavigateAsync(PageManager.PurchaseedProduct);
            }
            else
            {
                await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
            }
        }
        #endregion

        #region NavigateWalletBalancePage
        public ICommand NavigateWalletBalancePage { get; set; }
        private async void NavigateWalletBalancePageExcute()
        {
            if (IsLogin)
            {
                await Navigation.NavigateAsync(PageManager.WalletBalancePage);
            }
            else
            {
                await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
            }
        }

        #endregion

        #region LogOutCommand
        public ICommand LogOutCommand { get; set; }

        private async void LogOutExcute()
        {
            await CheckBusy(async () => {
                SqLiteService.DeleteAll<UserModel>();
                SqLiteService.DeleteAll<NotificationModel>();
                App.Settings.IsLogin = false;
                IsAdmin = false;
                IsLogin = false;
                SqLiteService.Update(App.Settings);
            });
        }

        #endregion

        #region UploadItemCommand
        public ICommand UploadItemCommand { get; set; }
        private async void UploadItemExcute()
        {
            if (IsLogin && IsAdmin)
            {
                await Navigation.NavigateAsync(PageManager.UploadProductPage);
            }
            else
            {
                await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
            }
        }
        #endregion
    }
}
