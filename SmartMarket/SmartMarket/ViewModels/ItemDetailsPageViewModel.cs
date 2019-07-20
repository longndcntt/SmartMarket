using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Services;
using SmartMarket.Enums;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
using SmartMarket.Models.API;
using SmartMarket.Services.HttpService;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using SmartMarket.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class ItemDetailsPageViewModel : ViewModelBase
    {
        #region constructor

        #endregion
        public ItemDetailsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
            : base(navigationService: navigationService, dialogService: pageDialogService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {
            ShowMoreItemDetailCommand = new DelegateCommand(ShowMoreItemDetailExcute);
            NavigateToCartCommand = new DelegateCommand(NavigateToCartExcute);
            AddToCartCommand = new DelegateCommand(AddToCartExcute);
            SubmitReviewCommand = new DelegateCommand(SubmitReviewExcute);

            if (App.Settings.IsLogin)
                UserInfo = SqLiteService.Get<UserModel>(x => x.Id != -1);
        }


        #region Properties
        private string _count = "1";
        public string Count
        {
            get => _count;
            set
            {
                SetProperty(ref _count, value);
            }
        }

        private int _ratingTotal = 5;
        public int RatingTotal
        {
            get => _ratingTotal;
            set
            {
                SetProperty(ref _ratingTotal, value);
            }
        }

        private string _contentOfReview;
        public string ContentOfReview
        {
            get => _contentOfReview;
            set
            {
                SetProperty(ref _contentOfReview, value);
            }
        }

        private int _position = 0;
        public int Position
        {
            get => _position;
            set
            {
                SetProperty(ref _position, value);
            }
        }

        private bool _isExistReview;
        public bool IsExistReview
        {
            get => _isExistReview;
            set
            {
                SetProperty(ref _isExistReview, value);
            }
        }

        private int _itemModelId;
        public int ItemModelId
        {
            get => _itemModelId;
            set => SetProperty(ref _itemModelId, value);
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private ItemModel _itemSelected;
        public ItemModel ItemSelected
        {
            get => _itemSelected;
            set => SetProperty(ref _itemSelected, value);
        }

        private ItemDetails _itemSelectedDetails;
        public ItemDetails ItemSelectedDetails
        {
            get => _itemSelectedDetails;
            set => SetProperty(ref _itemSelectedDetails, value);
        }

        private ObservableCollection<ImageItemDetail> _itemSelectedImage;
        public ObservableCollection<ImageItemDetail> ItemSelectedImage
        {
            get => _itemSelectedImage;
            set => SetProperty(ref _itemSelectedImage, value);
        }


        private ObservableCollection<ReviewProduct> _reviewProductList;
        public ObservableCollection<ReviewProduct> ReviewProductList
        {
            get => _reviewProductList;
            set => SetProperty(ref _reviewProductList, value);
        }


        #endregion

        #region Navigate
        public async override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                int itemID = -1;
                if (parameters.ContainsKey(ParamKey.SelectedItemId.ToString()))
                {
                    // SelectedCategory = (Category)parameters[ParamKey.Category.ToString()];
                    ItemModelId = (int)parameters[ParamKey.SelectedItemId.ToString()];
                }
                if (parameters.ContainsKey(ParamKey.SelectedItem.ToString()))
                {
                    ItemSelected = (ItemModel)parameters[ParamKey.SelectedItem.ToString()];
                    ItemModelId = ItemSelected.Id;
                }
                if (ItemModelId != -1)
                {
                    var url = ApiUrl.GetItemDetails() + ItemModelId;
                    await LoadingPopup.Instance.Show();
                    var response = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
                    await GetItemCallBack(response);
                    //ItemSelectedDetails = new ItemDetails()
                    //{
                    //    Count = 5,
                    //    Manufacturer = "Japan",
                    //    ProductId = ItemSelected.Id,
                    //    Detail = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                    //    " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                    //    " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen",
                    //};

                    //ItemSelectedImage = new ObservableCollection<ImageItemDetail>()
                    //{
                    //    new ImageItemDetail()
                    //    {
                    //        ProductID = ItemSelected.Id,
                    //        Image = "sony_product"
                    //    },
                    //    new ImageItemDetail()
                    //    {
                    //        ProductID = ItemSelected.Id,
                    //        Image = "sony_product"
                    //    },
                    //    new ImageItemDetail()
                    //    {
                    //        ProductID = ItemSelected.Id,
                    //        Image = "sony_product"
                    //    }

                }

                //ReviewProductList = new ObservableCollection<ReviewProduct>()
                //    {
                //        new ReviewProduct()
                //        {
                //            ProductId = itemID,
                //            Rate = 3,
                //            ReviewedDateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                //            Content = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                //        " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen.",
                //        },
                //        new ReviewProduct()
                //        {
                //            ProductId = itemID,
                //            Rate = 5,
                //            ReviewedDateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                //            Content = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                //        " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen.",
                //        },
                //        new ReviewProduct()
                //        {
                //            ProductId = itemID,
                //            Rate = 4,
                //            ReviewedDateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                //            Content = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                //        " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen.",
                //        }
                //    };
                //var itemSelectedDetailsTemp = SqLiteService.GetList<ItemDetails>(item => item.ProductId == ItemSelected.Id).ToList();
                //if (itemSelectedDetailsTemp.Count == 0)
                //{
                //    ItemSelectedDetails = new ItemDetails()
                //    {
                //        Count =5,
                //        Manufacturer = "Japan",
                //        ProductId = ItemSelected.Id,
                //        Detail = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen",
                //    };
                //}
                //else
                //{
                //    ItemSelectedDetails = SqLiteService.Get<ItemDetails>(item => item.ProductId == ItemSelected.Id);
                //}
            }
        }

        private async Task GetItemCallBack(ModelRestFul response)
        {
            if (response.ErrorCode != 200)
            {
                await MessagePopup.Instance.Show("Fail");
                return;
            }
            ItemSelectedDetails = response.Deserialize<ItemDetails>(response.Result);
            await LoadingPopup.Instance.Hide();
            await LoadReview();
        }

        //Get review of product
        private async Task LoadReview()
        {
            var url = ApiUrl.GetReview(ItemModelId.ToString());
            var reviewTemp = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
            var a = reviewTemp.Deserialize<IEnumerable<ReviewProduct>>(reviewTemp.Result).ToList();
            if (a.Count != -1)
            {
                ReviewProductList = new ObservableCollection<ReviewProduct>(a);
                if (ReviewProductList.Count <= 0)
                {
                    IsExistReview = false;
                    RatingTotal = 0;
                }
                else
                {
                    RatingTotal = (int)(ReviewProductList.Sum(x => x.Rate) / ReviewProductList.Count());
                    IsExistReview = true;
                }
            }
        }
        #endregion

        #region LoadItemSelected
        private async Task LoadItemSelected(int itemId)
        {
            var url = ApiUrl.GetItemSelected(itemId.ToString());
            var tempItem = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
            var a = tempItem.Deserialize<ItemModel>(tempItem.Result);
            ItemSelected = a;
        }
        #endregion

        #region ShowMoreItemDetailCommand
        public ICommand ShowMoreItemDetailCommand { get; set; }

        private async void ShowMoreItemDetailExcute()
        {
            await MessagePopup.Instance.Show(ItemSelectedDetails.Detail);
        }

        #endregion

        #region AddToCartCommand
        private bool isExist = false;
        public ICommand AddToCartCommand { get; set; }
        private async void AddToCartExcute()
        {
            if (ItemSelected == null)
            {
                await LoadItemSelected(ItemModelId);
            }
            var oderDetails = new OrderDetails()
            {
                Id = DateTime.Now.ToString(),
                Amount = Int32.Parse(Count),
                Name = ItemSelected.ItemName,
                ProductId = ItemSelected.Id,
                Image = ItemSelected.Image,
                Price = ItemSelected.Price,
                Manufacturer = ItemSelectedDetails.Manufacturer,
            };
            if (!IsNullCart)
            {
                IsNullCart = false;
                var listItemOfCartTemp = SqLiteService.GetList<OrderDetails>(x => x.Id != "");
                ItemOfOrder = new ObservableCollection<OrderDetails>();
                if (listItemOfCartTemp != null)
                {
                    foreach (var item in listItemOfCartTemp)
                    {
                        isExist = false;
                        if (oderDetails.ProductId == item.ProductId)
                        {
                            item.Amount = oderDetails.Amount + item.Amount;
                            SqLiteService.Update(item);
                            isExist = true;
                            break;
                        }
                    }
                    ItemOfOrder = new ObservableCollection<OrderDetails>(listItemOfCartTemp);
                }
                if (!isExist)
                {
                    var a = SqLiteService.Insert(oderDetails);
                }
            }
        }

        #endregion

        #region NavigateToCartCommand
        public ICommand NavigateToCartCommand { get; set; }
        private async void NavigateToCartExcute()
        {
            if (ItemSelected == null)
            {
                await LoadItemSelected(ItemModelId);
            }
            var oderDetails = new OrderDetails()
            {
                Id = DateTime.Now.ToString(),
                Amount = Int32.Parse(Count),
                Name = ItemSelected.ItemName,
                ProductId = ItemSelected.Id,
                Price = ItemSelected.Price,
                Image = ItemSelected.Image,
                Manufacturer = ItemSelectedDetails.Manufacturer,
            };
            if (!IsNullCart)
            {
                IsNullCart = false;
                var listItemOfCartTemp = SqLiteService.GetList<OrderDetails>(x => x.Id != "");
                ItemOfOrder = new ObservableCollection<OrderDetails>();
                if (listItemOfCartTemp != null)
                {
                    foreach (var item in listItemOfCartTemp)
                    {
                        isExist = false;
                        if (oderDetails.ProductId == item.ProductId)
                        {
                            item.Amount = oderDetails.Amount + item.Amount;
                            SqLiteService.Update(item);
                            isExist = true;
                            break;
                        }
                    }
                    ItemOfOrder = new ObservableCollection<OrderDetails>(listItemOfCartTemp);
                }
                if (!isExist)
                {
                    var a = SqLiteService.Insert(oderDetails);
                }
            }
            await Navigation.NavigateAsync(PageManager.ShowCardPage);
        }
        #endregion

        public int Value { get; set; }

        #region SubmitReviewCommand
        public ICommand SubmitReviewCommand { get; set; }

        private async void SubmitReviewExcute()
        {
            await CheckBusy(async () =>
            {
                try
                {
                    await LoadingPopup.Instance.Show();
                    var url = ApiUrl.SubmitReview();
                    if (UserInfo == null)
                    {
                        await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                        {
                            await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
                            return;
                        });
                    }
                    var review = new ReviewProduct()
                    {
                        WalletAddress = UserInfo.WalletAddress,
                        ProductId = ItemModelId,
                        Rate = Value,
                        Content = ContentOfReview,
                    };
                    var httpContent = review.ObjectToStringContent();
                    var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
                    await PostReviewCallBack(response);
                }
                catch (Exception)
                {

                }
                finally
                {
                    await LoadingPopup.Instance.Hide();
                }
            });
        }

        private async Task PostReviewCallBack(ModelRestFul response)
        {
            if (response.ErrorCode != 200)
            {
                await MessagePopup.Instance.Show("Fail");
                return;
            }
            var transaction = response.Deserialize<SmartMarket.Models.API.Transaction>(response.Result);
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
                return;
                // get event list fail
                //await MessagePopup.Instance.Show(TranslateExtension.Get("GetListEventsFailed"));
            }
            // get event list successfull
            var transaction = response.Deserialize<TransactionIDModel>(response.Result);
            if (transaction != null)
            {
                await LoadReview();
                await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                {
                    await MessagePopup.Instance.Show("Upload success");
                });
            }
            await LoadingPopup.Instance.Hide();
        }
        #endregion

        #region Zoom

        protected override async void ZoomImageExe(object obj)
        {
            await CheckBusy(async () =>
            {
                if (obj == null)
                {
#if DEBUG
                    Debug.WriteLine("NULL");
#endif
                    return;
                }

                await PopupNavigation.Instance.PushAsync(new ZoomImagePopUp(ItemSelectedDetails.Images[Position]));
            });
        }

        #endregion

    }
}
