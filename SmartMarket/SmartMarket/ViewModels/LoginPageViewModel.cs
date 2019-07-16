using Prism.Commands;
using Prism.Mvvm;
using SmartMarket.Models.API;
using Prism.Navigation;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.Services.HttpService;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using SmartMarket.Views.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using Prism.Services;

namespace SmartMarket.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Properties
        #endregion
        private bool _isAdmin = false;

        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }


        #region Constructor
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IHttpRequest httpRequest, ISqLiteService sqliteService)
            : base(navigationService: navigationService, dialogService: pageDialogService, httpRequest: httpRequest, sqliteService: sqliteService)
        {
            LoginCommand = new DelegateCommand(LoginExecute);
        }
        #endregion

        #region LoginCommand 

        public ICommand LoginCommand { get; set; }
        private async void LoginExecute()
        {
            await CheckBusy(async () =>
            {
                await Task.Run(async () =>
                 {
                     await LoadingPopup.Instance.Show(TranslateExtension.Get("Login"));
                     var url = ApiUrl.UserLogin();
                     //Username = "admin@gm.com";
                     //Password = "admin";
                     Username = "test1@gm.com";
                     Password = "123456";
                     var param = new UserIdentity
                     {
                         Email = Username,
                         Password = Password
                     };

                     var httpContent = param.ObjectToStringContent();
                     var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
                     await LoginCallBack(response);
                 });

            });
        }

        private async Task LoginCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                // login fail
                await MessagePopup.Instance.Show(TranslateExtension.Get("Username_PasswordIncorrect"));
                return;
            }
            try
            {
                var user = response.Deserialize<UserModel>(response.Result);
                if (user == null)
                {
                    // login fail
                    await MessagePopup.Instance.Show(TranslateExtension.Get("Username_PasswordIncorrect"));
                    return;
                }
                if (user.Email == "admin@gm.com" && user.Password == "admin")
                {
                    IsAdmin = true;
                }
                var walletAddress = Wallet.GetWallet(user.Keystore, user.Password);
                user.WalletAddress = walletAddress.ElementAt(0).Key.ToString();
                user.PrivateKey = walletAddress.ElementAt(0).Value.ToString();
                App.Settings.IsLogin = true;
                SqLiteService.Update(App.Settings);

                SqLiteService.Insert(user);
                await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                {
                    await Navigation.NavigateAsync(PageManager.TabbedMainPage);
                });
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            finally
            {
                await LoadingPopup.Instance.Hide();
            }
            // login successfull - get user info & navigate to upload page


            //IsShowLoginForm = false;

            //AddHeaderToken(token: user.AccessToken);


            ////await LoadingPopup.Instance.Hide();

            //await GetEventListExecute(user.ClientId);
        }
        #endregion



    }
}
