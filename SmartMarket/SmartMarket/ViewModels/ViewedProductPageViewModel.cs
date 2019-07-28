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
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
	public class ViewedProductPageViewModel : ViewModelBase
	{
       

        #region Properties
        private ObservableCollection<ItemModel> _viewedItemList;
        public ObservableCollection<ItemModel> ViewedItemList
        {
            get => _viewedItemList;
            set => SetProperty(ref _viewedItemList, value);
        }
        private ItemModel _selectedItem;
        public ItemModel SelectedItemTapped
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        #endregion
        #region Constructor
        public ViewedProductPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
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
                var b = SqLiteService.GetList<ItemModel>(x => x.ItemName != null || x.ItemName != "");
                if (b != null)
                {
                    ViewedItemList = new ObservableCollection<ItemModel>(b);
                }
                await LoadingPopup.Instance.Hide();

            }
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
                            {ParamKey.SelectedItem.ToString(), SelectedItemTapped},
                            {ParamKey.CategoryId.ToString(), SelectedItemTapped.CategoryId},
                         //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                        };

                    await Navigation.NavigateAsync(PageManager.ItemDetailsPage, parameters: param);
                }
            });
        }
        #endregion
    }
}
