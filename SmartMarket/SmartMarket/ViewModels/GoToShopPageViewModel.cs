using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Enums;
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class GoToShopPageViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<ItemAPIUpload> _allItemList;
        public ObservableCollection<ItemAPIUpload> AllItemList
        {
            get => _allItemList;
            set => SetProperty(ref _allItemList, value);
        }

        private ExchangeModel _selectedItem;
        public ExchangeModel SelectedItemTapped
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        #endregion

        #region Constructors
        public GoToShopPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
           : base(navigationService: navigationService, dialogService: pageDialogService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {
            ItemTappedCommand = new DelegateCommand(SelectedItemExcutWithoutPara);
            UserInfo = SqLiteService.Get<UserModel>(x => x.Id != -1);
        }
        #endregion

        #region OnAppear
        public async override void OnAppear()
        {
            base.OnAppear();
            if (!string.IsNullOrEmpty(UserInfo.WalletAddress))
            {
                await LoadingPopup.Instance.Show(TranslateExtension.Get("Loading3dot"));
                var url = ApiUrl.GetItemOfShop(UserInfo.WalletAddress);

                var response = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
                await GetWalletCallBack(response);
            }
        }

        private async Task GetWalletCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
                return;
            }
            else
            {
                Serializer serializer = new Serializer();
                var a = response.Result.ToString();
                var b = serializer.Deserialize<ObservableCollection<ItemAPIUpload>>(a);
                if (b != null)
                {
                    AllItemList = new ObservableCollection<ItemAPIUpload>(b);
                }
            }
            await LoadingPopup.Instance.Hide();
        }
        #endregion

        #region ItemSelected
        public ICommand ItemTappedCommand { get; set; }
        private async void SelectedItemExcutWithoutPara()
        {
            await CheckBusy(async () =>
            {
                if (SelectedItemTapped != null)
                {
                    // var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == SelectedItemTapped.Id);
                    //await MessagePopup.Instance.Show(SelectedItemTapped.Id.ToString());
                    var param = new NavigationParameters
                        {
                            {ParamKey.SelectedItemId.ToString(), SelectedItemTapped.productId},
                         //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                        };

                    await Navigation.NavigateAsync(PageManager.ItemDetailsPage, parameters: param);
                }
            });
        }
        #endregion

        #region EditItem
        //private async void EditItemExcute(string itemId)
        //{
        //    await CheckBusy(async () =>
        //    {
        //        if (!string.IsNullOrEmpty(itemId))
        //        {
        //            // var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == SelectedItemTapped.Id);
        //            //await MessagePopup.Instance.Show(SelectedItemTapped.Id.ToString());
        //            var param = new NavigationParameters
        //                {
        //                    {ParamKey.SelectedItemId.ToString(), itemId},
        //                    {ParamKey.IsEdit.ToString(), true},
        //                 //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
        //                };

        //            await Navigation.NavigateAsync(PageManager.UploadProductPage, parameters: param);
        //        }
        //    });
        //}
        #endregion

        #region DeleteItem
        public async void DeleteItemExcute(string itemId)
        {
            if (!string.IsNullOrEmpty(itemId))
            {
                await LoadingPopup.Instance.Show(TranslateExtension.Get("Loading3dot"));
                var url = ApiUrl.DeleteItem();
                var param = new ItemAPIUpload()
                {
                    WalletAddress = UserInfo.WalletAddress,
                    ProductId = Int32.Parse(itemId),
                };
                var httpContent = param.ObjectToStringContent();
                var response = await HttpRequest.PutTaskAsync<ModelRestFul>(url, httpContent);
                await DeleteItemCallBack(response, itemId);
            }
        }

        private async Task DeleteItemCallBack(ModelRestFul response, string itemId)
        {
            if (response.ErrorCode != 200)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
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
                    await UploadToBlockchain(stringSigned, itemId);
                }
            }
        }

        private async Task UploadToBlockchain(string stringSigned, string itemId)
        {
            var url = ApiUrl.UploadToBlockChain();
            var signed = new SignedTransaction(stringSigned);
            var httpContent = signed.ObjectToStringContent();
            var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
            await UploadToBlockchainCallBack(response, itemId);
        }

        private async Task UploadToBlockchainCallBack(ModelRestFul response, string itemId)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
                return;
                // get event list fail
                //await MessagePopup.Instance.Show(TranslateExtension.Get("GetListEventsFailed"));
            }
            // get event list successfull
            var transaction = response.Deserialize<TransactionIDModel>(response.Result);
            if (!string.IsNullOrEmpty(transaction.TransactionID))
            {
                var a = AllItemList.Where(x => x.Id == Int32.Parse(itemId)).FirstOrDefault();
                AllItemList.Remove(a);
                await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                {
                    await MessagePopup.Instance.Show(TranslateExtension.Get("DeleteSuccess"));
                });
            }
            await LoadingPopup.Instance.Hide();
        }
        #endregion
    }
}
