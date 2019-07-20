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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class PurchaseedProductViewModel : ViewModelBase
    {

        #region Properties
        private ObservableCollection<ItemModel> _puchasedItemList;
        public ObservableCollection<ItemModel> PuchasedItemList
        {
            get => _puchasedItemList;
            set => SetProperty(ref _puchasedItemList, value);
        }
        #endregion
        #region Constructor
        public PurchaseedProductViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
            : base(navigationService: navigationService, dialogService: pageDialogService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {
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
                var url = ApiUrl.GetPurchasedItems(UserInfo.WalletAddress);

                var response = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
                await GetWalletCallBack(response);
            }
        }

        private async Task GetWalletCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show("Fail");
            }
            else
            {
                Serializer serializer = new Serializer();
                var a = response.Result.ToString();
                var b = serializer.Deserialize<ObservableCollection<ItemModel>>(a);
                if (b != null)
                {
                    PuchasedItemList = new ObservableCollection<ItemModel>(b);
                }
            }
            await LoadingPopup.Instance.Hide();
        }
        #endregion

        #region ItemSelected
        public ICommand ItemTappedCommand { get; set; }
        public async void SelectedItemExcute(string itemId)
        {
            await CheckBusy(async () =>
            {
                if (!string.IsNullOrEmpty(itemId))
                {
                    var iteNumberId = Int32.Parse(itemId);
                    var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == iteNumberId);
                    var selectedCategory = SqLiteService.Get<Category>(x => x.Id == selectedItem.CategoryId);
                    var param = new NavigationParameters
                    {
                        {ParamKey.SelectedItem.ToString(), selectedItem},
                        {ParamKey.Category.ToString(), selectedCategory},
                    //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                    };

                    await Navigation.NavigateAsync(PageManager.ItemDetailsPage, parameters: param);
                }
            });
        }
        #endregion

    }
}
