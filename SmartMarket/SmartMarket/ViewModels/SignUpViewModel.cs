using Prism.Commands;
using Prism.Mvvm;
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

namespace SmartMarket.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        #region constructors
        public SignUpViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
       : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {
            SignUpCommand = new DelegateCommand(SignupExcute);
        }
        #endregion

        #region Properties
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _keystore;
        public string Keystore
        {
            get => _keystore;
            set => SetProperty(ref _keystore, value);
        }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private string _dayofBirth;
        public string DayofBirth
        {
            get => _dayofBirth;
            set => SetProperty(ref _dayofBirth, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }
        #endregion

        #region SignUpCommand
        public ICommand SignUpCommand { get; set; }
        private async void SignupExcute()
        {
            await CheckBusy(async () =>
            {
                await LoadingPopup.Instance.Show(TranslateExtension.Get("Login"));
                if (string.IsNullOrEmpty(Email))
                {
                    await MessagePopup.Instance.Show(TranslateExtension.Get("UsernameEmpty"));
                    return;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    await MessagePopup.Instance.Show(TranslateExtension.Get("PasswordEmpty"));
                    return;
                }

                await Task.Run(async () =>
                {
                    var url = ApiUrl.UserRegister();

                    //Create Keystore
                    Keystore = Wallet.CreateWallet(Password);

                    //Encrypt Password
                    Password = Wallet.CryptPassword(Password);

                    var userRegister = new UserModel()
                    {
                        Email = Email,
                        Password = Password,
                        Keystore = Keystore,
                        FullName = FullName,
                        DayofBirth = DayofBirth,
                        Gender = true,
                        Address = Address,
                        NumberID = "012345678",
                        PhoneNumber = PhoneNumber,

                        PortraitImage = string.Empty,
                        IdentityImage = string.Empty,
                    };

                    var httpContent = userRegister.ObjectToStringContent();
                    ModelRestFul test = new ModelRestFul();
                    var a =test.Serialize<object>(Keystore);
                    var response = await HttpRequest.PutTaskAsync<UserModel>(url, httpContent);
                    await SigupCallBack(response);
                });
            });

        }

        private async Task SigupCallBack(UserModel response)
        {
            
        }
        #endregion
    }
}