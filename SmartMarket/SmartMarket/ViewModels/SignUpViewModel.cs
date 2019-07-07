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
        public string DateOfBirth
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

        private int _selectedGender;
        public int SelectedGender
        {
            get => _selectedGender;
            set => SetProperty(ref _selectedGender, value);
        }
        #endregion

        #region SignUpCommand
        public ICommand SignUpCommand { get; set; }
        private async void SignupExcute()
        {
            await CheckBusy(async () =>
            {

                Email = "test123@gm.com";
                Password = "123456";
                FullName = "LongND";
                Address = "abc";
                PhoneNumber = "123456789";
                await LoadingPopup.Instance.Show(TranslateExtension.Get("SignUp"));
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
                    //Password = Wallet.CryptPassword(Password);


                    var userRegister = new UserModel()
                    {
                        Email = Email,
                        Password = Password,
                        Keystore = Keystore,
                        FullName = FullName,
                        DayOfBirth = DateOfBirth,
                        Gender = SelectedGender == 0 ? true : false,
                        Address = "abc",
                        NumberID = "012345678",
                        PhoneNumber = PhoneNumber,

                        PortraitImage = string.Empty,
                        IdentityImage = string.Empty,
                    };

                    var httpContent = userRegister.ObjectToStringContent();
                    ModelRestFul test = new ModelRestFul();
                    var a =test.Serialize<object>(Keystore);
                    var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
                    await SigupCallBack(response);
                });
            });

        }

        private async Task SigupCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show("Fail");
            }
            else
            {
                try
                {
                    var transaction = response.Deserialize<Transaction>(response.Result);
                    if (transaction != null)
                    {
                        //  Password = Wallet.DecryptPassword(Password, DateTime.UtcNow.ToString("yyyy-MM-dd"));
                        //  Password = Wallet.DecryptPassword(Password, DateTime.UtcNow.ToString("yyyy-MM-dd"));
                        var walletAddress = Wallet.GetWallet(Keystore, Password);
                        var privatekey = walletAddress.ElementAt(0).Value.ToString();
                        var signer = new Signer();
                        var stringSigned = signer.Sign(privatekey, transaction);
                        //var transactionID = transaction.Transaction;
                        if (!string.IsNullOrEmpty(stringSigned))
                        {
                            await UploadToBlockchain(stringSigned);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Write(e.Message);
                }
                finally
                {
                    await LoadingPopup.Instance.Hide();
                }
            }
        }
        private async Task UploadToBlockchain(string stringSigned)
        {
            var url = ApiUrl.UploadToBlockChain();
            var signed = new SignedTransaction(stringSigned);
            var httpContent = signed.ObjectToStringContent();
            var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
            await UploadToBlockchainCallBack(response);
        }

        private async Task UploadToBlockchainCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show("Fail");
                // get event list fail
                //await MessagePopup.Instance.Show(TranslateExtension.Get("GetListEventsFailed"));
            }
            else
            {
                // get event list successfull
                var transaction = response.Deserialize<TransactionIDModel>(response.Result);
                if (transaction != null)
                {
                    var transactionID = transaction.TransactionID;
                    await LoadingPopup.Instance.Hide();
               
                    await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                    {
                        await Navigation.NavigateAsync(PageManager.TabbedMainPage);
                    });

                }
            }
            await LoadingPopup.Instance.Hide();
        }

        #endregion
    }
}