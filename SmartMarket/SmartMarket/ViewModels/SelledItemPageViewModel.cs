using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Enums;
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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
	public class SelledItemPageViewModel : ViewModelBase
	{
        #region Properties
        private ObservableCollection<ExchangeModel> _selledItemList;
        public ObservableCollection<ExchangeModel> SelledItemList
        {
            get => _selledItemList;
            set => SetProperty(ref _selledItemList, value);
        }

        private ExchangeModel _selectedItem;
        public ExchangeModel SelectedItemTapped
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        #endregion
        #region Constructor
        public SelledItemPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
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
                var url = ApiUrl.GetSelledItems(UserInfo.WalletAddress);

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
                var b = serializer.Deserialize<ObservableCollection<ExchangeModel>>(a);
                if (b != null)
                {
                   SelledItemList = new ObservableCollection<ExchangeModel>(b.OrderByDescending(x=>x.unixtime));
                    foreach (var item in SelledItemList)
                    {
                        item.StatusExchange = (item.remain > 0) ? (item.isDone) ? Color.Red : Color.Orange : Color.LightGreen;
                        item.StatusExchangeString = (item.remain > 0) ? (item.isDone) ? TranslateExtension.Get("PaymentFail") : TranslateExtension.Get("PaymentPending") : TranslateExtension.Get("Success");
                    }
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
    }
}
