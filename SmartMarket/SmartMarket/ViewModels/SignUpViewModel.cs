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
using SmartMarket.Enums;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.IO;
using Xamarin.Forms;
using SmartMarket.Files;

namespace SmartMarket.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        #region constructors
        public SignUpViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest, IFileService fileService)
       : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest, fileService: fileService)
        {
            SignUpCommand = new DelegateCommand(SignupExcute);
            EditCommand = new DelegateCommand(EditUserExcute);
            ChoosePhotoCommand = new DelegateCommand(async () => await ChoosePhotoReceiveExecute());
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

        private byte[] _imageStream;

        public byte[] ImageStream
        {
            get => _imageStream;
            set => SetProperty(ref _imageStream, value);
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

        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        private ImageSource _portraitImageSource;
        public ImageSource PortraitImageSource
        {
            get => _portraitImageSource;
            set => SetProperty(ref _portraitImageSource, value);
        }

        private string _portraitImageBase;
        public string PortraitImageBase
        {
            get => _portraitImageBase;
            set => SetProperty(ref _portraitImageBase, value);
        }
        #endregion

        #region Navigate

        public override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            base.OnNavigatedNewToAsync(parameters);
            if (parameters != null)
            {
                UserInfo = (UserModel)parameters[ParamKey.UserInfo.ToString()];
                IsEdit = (bool)parameters[ParamKey.IsEdit.ToString()];
                if (UserInfo != null)
                {
                    Email = UserInfo.Email;
                    Password = UserInfo.Password;
                    FullName = UserInfo.FullName;
                    Address = UserInfo.Address;
                    PhoneNumber = UserInfo.PhoneNumber;
                    SelectedGender = UserInfo.Gender ? 1 : 0;
                    DateOfBirth = UserInfo.DayOfBirth;
                    Keystore = UserInfo.Keystore;
                }
            }
        }
        #endregion

        #region SignUpCommand
        public ICommand SignUpCommand { get; set; }
        private async void SignupExcute()
        {
            await CheckBusy(async () =>
            {

                //Email = "PhucNH@gm.com";
                //Password = "123456";
                //FullName = "PhucNH";
                //Address = "KTX khu B";
                //PhoneNumber = "123456789";
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
                        Address = Address,
                        NumberID = "012345678",
                        PhoneNumber = PhoneNumber,

                        Image = PortraitImageBase,
                        //IdentityImage = string.Empty,
                    };

                    var httpContent = userRegister.ObjectToStringContent();
                    ModelRestFul test = new ModelRestFul();
                    var a = test.Serialize<object>(Keystore);
                    var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
                    await SigupCallBack(response);
                });
            });

        }

        private async Task SigupCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
                return;
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
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
                return;
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
                        if (!IsEdit)
                        {
                            await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
                           
                        }
                        else
                        {
                            var url = ApiUrl.UserLogin();
                            //Username = "admin@gm.com";
                            //Password = "admin";
                            //Username = "test1@gm.com";
                            //Password = "123456";
                            //Username = "PhucNH@gm.com";
                            //Password = "123456";
                            var param = new UserIdentity
                            {
                                Email = Email,
                                Password = Password
                            };

                            var httpContent = param.ObjectToStringContent();
                            var loginResponse = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
                            await LoginCallBack(loginResponse);
                            await Navigation.GoBackToRootAsync();
                        }
                    });

                }
            }
            await LoadingPopup.Instance.Hide();
        }

        #endregion

        #region Choose Photo

        private bool _choosePhoto = true;
        private bool _takePhoto = true;

        public ICommand ChoosePhotoCommand { get; }

        public async Task ChoosePhotoExecute()
        {
            if (!_takePhoto || !_choosePhoto)
                return;

            _choosePhoto = false;

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await MessagePopup.Instance.Show(message: "No Pick Image Support", closeButtonText: "OK");
            }
            else
            {
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (storageStatus != PermissionStatus.Granted)
                {
                    var results =
                        await
                            CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    storageStatus = results[Permission.Storage];
                }

                if (storageStatus == PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.PickPhotoAsync();

                    //Small delay
                    await Task.Delay(TimeSpan.FromMilliseconds(150));

                    if (file == null)
                    {
                        _choosePhoto = true;
                        return;
                    }

                    var memoryStream = new MemoryStream();
                    file.GetStream().CopyTo(memoryStream);

                    byte[] image = memoryStream.ToArray();
                    var resizeImage = await FileService.ResizeImage(image, file.Path, 4);
                    PortraitImageBase = Convert.ToBase64String(resizeImage);
                    await ChangeImage(file.Path, image);

                    //dispose mediafile
                    file.Dispose();
                }
                else
                {
                    await MessagePopup.Instance.Show(message: "Permission", closeButtonText: "OK");
                    //On iOS you may want to send your user to the settings screen.
                    CrossPermissions.Current.OpenAppSettings();
                }
            }

            _choosePhoto = true;
            await CrossMedia.Current.Initialize();
        }

        private string _imagePath;
        public async Task ChangeImage(string filePath, byte[] bytes)
        {
            _imagePath = filePath;
            ImageStream = bytes;
        }

        private async Task ChoosePhotoReceiveExecute()
        {
            await ChoosePhotoExecute();
            PortraitImageSource = ImageSource.FromStream(() => new MemoryStream(ImageStream));
        }
        #endregion

        #region EditUserInfo
        public ICommand EditCommand { get; set; }
        private async void EditUserExcute()
        {
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
            if (string.IsNullOrEmpty(PortraitImageBase))
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("ImageEmpty"));
                return;
            }


            await Task.Run(async () =>
             {
                 var url = ApiUrl.EditUser();

                //Create Keystore
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
                     Address = Address,
                     NumberID = "012345678",
                     PhoneNumber = PhoneNumber,
                     WalletAddress = UserInfo.WalletAddress,
                     Image = PortraitImageBase,
                    //IdentityImage = string.Empty,
                };

                 var httpContent = userRegister.ObjectToStringContent();
                 ModelRestFul test = new ModelRestFul();
                 var a = test.Serialize<object>(Keystore);
                 var response = await HttpRequest.PutTaskAsync<ModelRestFul>(url, httpContent);
                 await EditCallBack(response);
             });
        }

        private async Task EditCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
                return;
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

                        var signer = new Signer();
                        var stringSigned = signer.Sign(UserInfo.PrivateKey, transaction);
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
        #endregion

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
                user.WalletAddress = UserInfo.WalletAddress;
                user.PrivateKey = UserInfo.PrivateKey;
                App.Settings.IsLogin = true;
                SqLiteService.Update(App.Settings);

                SqLiteService.Update(user);
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
    }
}