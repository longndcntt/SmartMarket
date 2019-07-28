using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Enums;
using SmartMarket.Files;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.Models.API;
using SmartMarket.Services.HttpService;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using SmartMarket.Views.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMarket.ViewModels
{
    public class UploadProductPageViewModel : ViewModelBase
    {
        #region Properties
        private ImageSource _image;

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private ObservableCollection<ListImage> _listImage;

        public ObservableCollection<ListImage> ListImage
        {
            get => _listImage;
            set => SetProperty(ref _listImage, value);
        }

        private byte[] _imageStream;

        public byte[] ImageStream
        {
            get => _imageStream;
            set => SetProperty(ref _imageStream, value);
        }

        private List<byte[]> _listimageStream;

        public List<byte[]> ListImageStream
        {
            get => _listimageStream;
            set => SetProperty(ref _listimageStream, value);
        }

        private string _itemName;

        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        private string _price;

        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private string _count;

        public string Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        private string _Manufacturer;

        public string Manufacturer
        {
            get => _Manufacturer;
            set => SetProperty(ref _Manufacturer, value);
        }

        private string _Detail;

        public string Detail
        {
            get => _Detail;
            set => SetProperty(ref _Detail, value);
        }

        private ObservableCollection<Category> _categoryList;

        public ObservableCollection<Category> CategoryList
        {
            get => _categoryList;
            set => SetProperty(ref _categoryList, value);
        }

        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private string _imagePath;

        public async Task ChangeImage(string filePath, byte[] bytes)
        {
            _imagePath = filePath;
            ImageStream = bytes;
        }

        public ICommand TakeSomePhotosReceiveCommand { get; }
        private async Task TakeSomePhotosReceiveExecute()
        {
            await TakeSomePhotosExecute();
            for (int i = 0; i < ListImageStream.Count; i++)
            {
                var image = new ListImage()
                {
                    ImageList = ImageSource.FromStream(() => new MemoryStream(ListImageStream[i])),
                };
                ListImage.Add(image);
            }
        }
        public ICommand ChooseSomePhotosReceiveCommand { get; }

        private async Task ChooseSomesPhotosReceiveExecute()
        {
            await ChooseSomePhotosExecute();
            for (int i = 0; i < ListImageStream.Count; i++)
            {
                var image = new ListImage()
                {
                    ImageList = ImageSource.FromStream(() => new MemoryStream(ListImageStream[i])),
                };
                ListImage.Add(image);
            }
        }

        public ICommand TakePhotoReceiveCommand { get; }

        private async Task TakePhotoReceiveExecute()
        {
            await TakePhotoExecute();
            Image = ImageSource.FromStream(() => new MemoryStream(ImageStream));
        }

        public string BaseImage { get; set; }

        public ICommand ChoosePhotoReceiveCommand { get; }

        private async Task ChoosePhotoReceiveExecute()
        {
            await ChoosePhotoExecute();
            Image = ImageSource.FromStream(() => new MemoryStream(ImageStream));

        }
        #endregion
        public UploadProductPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService,
            IHttpRequest httpRequest, IPageDialogService dialogService, IFileService fileService)
        : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest, dialogService: dialogService, fileService: fileService)
        {
            TakePhotoReceiveCommand = new DelegateCommand(async () => await TakePhotoReceiveExecute());
            ChoosePhotoReceiveCommand = new DelegateCommand(async () => await ChoosePhotoReceiveExecute());
            TakeSomePhotosReceiveCommand = new DelegateCommand(async () => await TakeSomePhotosReceiveExecute());
            ChooseSomePhotosReceiveCommand = new DelegateCommand(async () => await ChooseSomesPhotosReceiveExecute());
            UploadItemCommand = new DelegateCommand(UploadItemExcute);
            //ItemName = "Test 2";
            //Price = "40";
            //Count = "5";
            //Manufacturer = "Manufacturer 2";
            //Detail = "Detail 2";

        }

        #region Choose Photo

        private bool _choosePhoto = true;

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
                    BaseImage = Convert.ToBase64String(resizeImage);
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

        #endregion

        #region Take Photo

        private bool _takePhoto = true;

        public ICommand TakePhotoCommand { get; }

        public async Task TakePhotoExecute()
        {
            if (!_takePhoto || !_choosePhoto)
                return;

            _takePhoto = false;

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await MessagePopup.Instance.Show(message: "No Camera", closeButtonText: "OK");
            }
            else
            {
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results =
                        await
                            CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        SaveToAlbum = true,
                        Directory = "Image1",
                        Name = DateTime.Now.ToString("MM/dd/yyyy"),
                    });

                    //Small delay
                    await Task.Delay(TimeSpan.FromMilliseconds(200));

                    if (file == null)
                    {
                        _takePhoto = true;
                        return;
                    }

                    var memoryStream = new MemoryStream();
                    file.GetStream().CopyTo(memoryStream);

                    byte[] image = memoryStream.ToArray();
                    var resizeImage = await FileService.ResizeImage(image, file.Path, 4);
                    BaseImage = Convert.ToBase64String(resizeImage);
                    await ChangeImage(file.Path, image);

                    //dispose mediafile
                    file.Dispose();
                }
                else
                {
                    //On iOS you may want to send your user to the settings screen.
                    CrossPermissions.Current.OpenAppSettings();
                }
            }

            _takePhoto = true;
            await CrossMedia.Current.Initialize();
        }

        #endregion

        #region OnAppear
        public override void OnAppear()
        {
            base.OnAppear();
            var list = SqLiteService.GetList<Category>(x => x.Id != -1).ToList();
            UserInfo = SqLiteService.Get<UserModel>(x => x.Id != -1);
            if (list.Count != 0)
            {
                CategoryList = new ObservableCollection<Category>(list);
               // SelectedCategory = CategoryList.First();
            }
        }
        #endregion

        #region UploadItemCommand
        public ICommand UploadItemCommand { get; set; }
        private async void UploadItemExcute()
        {
            await CheckBusy(async () =>
            {
                await LoadingPopup.Instance.Show();
                var url = ApiUrl.UploadItem();
                UserInfo = SqLiteService.Get<UserModel>(x => x.Id != -1);
                var itemModel = new ItemAPIUpload()
                {
                    CategoryId = SelectedCategory.Id,
                    ItemName = ItemName,
                    Price = Int32.Parse(Price),
                    Count = Int32.Parse(Count),
                    Image = BaseImage,
                    Manufacturer = Manufacturer,
                    Detail = Detail,
                    WalletAddress = UserInfo.WalletAddress,
                    Images = ListBase,
                };
                var httpContent = itemModel.ObjectToStringContent();
                var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
                await UploadItemExcuteCallBack(response);
            });
        }

        private async Task UploadItemExcuteCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
            }
            else
            {
                var transaction = response.Deserialize<Transaction>(response.Result);
                if (transaction != null)
                {
                    var signer = new Signer();
                    var privatekey = UserInfo.PrivateKey;
                    var stringSigned = signer.Sign(privatekey, transaction);
                    //var transactionID = transaction.Transaction;
                    if (!string.IsNullOrEmpty(stringSigned))
                    {
                        await UploadToBlockchain(stringSigned);
                    }
                }
            }
        }

        private async Task UploadToBlockchain(string stringSigned)
        {
            try
            {
                var url = ApiUrl.UploadToBlockChain();
                var signed = new SignedTransaction(stringSigned);
                var httpContent = signed.ObjectToStringContent();
                var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
                await UploadToBlockchainCallBack(response);
            }
            catch (Exception)
            {

            }
            finally
            {
                await LoadingPopup.Instance.Hide();
            }

        }

        private async Task UploadToBlockchainCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
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
                    var param = new NavigationParameters
                {
                    {ParamKey.TransactionID.ToString(), transactionID},
                    //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                };
                    await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                    {
                        await Navigation.NavigateAsync(PageManager.MessagePage, param);
                    });

                }
            }
            await LoadingPopup.Instance.Hide();
        }
        #endregion

        #region Take Some Photos

        private List<string> ListBase;
        public async Task TakeSomePhotosExecute()
        {
            if (!_takePhoto || !_choosePhoto)
                return;

            _takePhoto = false;

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await MessagePopup.Instance.Show(message: "No Camera", closeButtonText: "OK");
            }
            else
            {
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results =
                        await
                            CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                {
                    ListBase = new List<string>();
                    ListImageStream = new List<byte[]>();
                    ListImage = new ObservableCollection<ListImage>();
                    //Small delay
                    await Task.Delay(TimeSpan.FromMilliseconds(200));
                    bool isCamera = true;
                    int i = 0;
                    while (isCamera && i < 3)
                    {
                        var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            SaveToAlbum = true,
                            Directory = "Image1",
                            Name = DateTime.Now.ToString("MM/dd/yyyy"),
                        });

                        if (file != null)
                        {
                            var memoryStream = new MemoryStream();
                            file.GetStream().CopyTo(memoryStream);

                            byte[] image = memoryStream.ToArray();
                            var resizeImage = await FileService.ResizeImage(image, file.Path, 4);
                            BaseImage = Convert.ToBase64String(resizeImage);
                            ListBase.Add(BaseImage);
                            ListImageStream.Add(image);
                            //await ChangeImage(file.Path, image);
                            //dispose mediafile
                            file.Dispose();
                            //_takePhoto = true;
                            //return;
                            i++;
                        }
                        else
                        {
                            isCamera = false;
                        }
                    }
                }
                else
                {
                    //On iOS you may want to send your user to the settings screen.
                    CrossPermissions.Current.OpenAppSettings();
                }
            }

            _takePhoto = true;
            await CrossMedia.Current.Initialize();
        }

        #endregion

        #region Choose Some Photos

        private bool _choosePhotos = true;


        public async Task ChooseSomePhotosExecute()
        {
            if (!_takePhoto || !_choosePhoto)
                return;

            _choosePhotos = false;

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
                    ListBase = new List<string>();
                    ListImageStream = new List<byte[]>();
                    ListImage = new ObservableCollection<ListImage>();
                    bool isChoose = true;
                    int i = 0;
                    while (isChoose && i <3)
                    {
                        var file = await CrossMedia.Current.PickPhotoAsync();
                        //Small delay
                        if (file != null)
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(150));
                            var memoryStream = new MemoryStream();
                            file.GetStream().CopyTo(memoryStream);

                            byte[] image = memoryStream.ToArray();
                            var resizeImage = await FileService.ResizeImage(image, file.Path, 4);
                            BaseImage = Convert.ToBase64String(resizeImage);
                            ListBase.Add(BaseImage);
                            ListImageStream.Add(image);
                            //await ChangeImage(file.Path, image);
                            //dispose mediafile
                            file.Dispose();
                        }
                        else
                        {
                            isChoose = false;
                        }
                    }
                    
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

        #endregion
    }

    public class ListImage
    {
        public ImageSource ImageList { get; set; }
    }
}
