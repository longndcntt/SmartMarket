using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.Services.HttpService;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using SmartMarket.Views.Popups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMarket.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Properties
        #endregion

        #region Delegate
        #endregion

        #region Constructor
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) 
            : base(navigationService: navigationService, dialogService: pageDialogService)
        {
            LoginCommand = new DelegateCommand(LoginExecute);
        }

        
        #endregion
        public override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            base.OnNavigatedNewToAsync(parameters);
        }
        #region LoginCommand 

        public ICommand LoginCommand { get; set; }
        private async void LoginExecute()
        {
            await CheckBusy(async () =>
            {
                //await Task.Run(async () =>
                // {
                //     var url = ApiUrl.Get();

                //     var param = new UserIdentity { UserName = Username };
                //     param.Password = param.CryptPassword(Password);

                //     var encodeString = "fotoscan:fotoscan";
                //     var encodedBasicToken = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(encodeString));

                //     var checkHeader = HttpRequest.DefaultRequestHeaders.Authorization;

                //     if (checkHeader != null)
                //     {
                //         HttpRequest.DefaultRequestHeaders.Remove("Authorization");
                //     }

                //    // Add Token to the Http Request header
                //    HttpRequest.DefaultRequestHeaders.Add("Authorization", "Basic " + encodedBasicToken);

                //     var httpContent = param.ObjectToStringContent();
                //     var response = await HttpRequest.PutTaskAsync<UserModel>(url, httpContent);
                //     await LoginCallBack(response);
                // });

            });
        }

        private async Task LoginCallBack(UserModel user)
        {
            //if (user == null)
            //{
            //    // login fail
            //    await MessagePopup.Instance.Show(TranslateExtension.Get("Username_PasswordIncorrect"));
            //    HttpRequest.DefaultRequestHeaders.Remove("Authorization");
            //    return;
            //}

            //// login successfull - get user info & navigate to upload page
            //App.Settings.IsLogin = true;
            //App.Settings.ClientId = user.ClientId;
            //SqLiteService.Update(App.Settings);

            //SqLiteService.Insert(user);
            //UserInfo = user;

            //IsShowLoginForm = false;

            //AddHeaderToken(token: user.AccessToken);


            ////await LoadingPopup.Instance.Hide();

            //await GetEventListExecute(user.ClientId);
        }

        #endregion

      

    }
}
