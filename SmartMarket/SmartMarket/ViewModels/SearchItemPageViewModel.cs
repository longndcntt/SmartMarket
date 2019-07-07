using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Enums;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class SearchItemPageViewModel : ViewModelBase
    {
        #region Constructor
        public SearchItemPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
           : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {
            ItemTappedCommand = new DelegateCommand(SelectedItemExcutWithoutPara);
        }
        #endregion

        #region Navigate
        public override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            base.OnNavigatedNewToAsync(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.ContentSearch.ToString()))
                {
                    SearchContent = (string)parameters[ParamKey.ContentSearch.ToString()];
                    if (!string.IsNullOrEmpty(SearchContent))
                    {
                        var listTemp = SqLiteService.GetList<ItemModel>(x => x.ItemName.ToLower().Contains(SearchContent)).ToList();


                        if (listTemp.Count > 0)
                        {
                            SearchItemList = new ObservableCollection<ItemModel>(listTemp);
                            IsExistedItem = true;
                        }
                        else
                        {
                            IsExistedItem = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region Properties
        private string _searchContent;
        public string SearchContent
        {
            get => _searchContent;
            set => SetProperty(ref _searchContent, value);
        }

        private ObservableCollection<ItemModel> _searchItemList;
        public ObservableCollection<ItemModel> SearchItemList
        {
            get => _searchItemList;
            set =>SetProperty(ref _searchItemList , value);
        }

        private bool _isExistedItem = true;
        public bool IsExistedItem
        {
            get => _isExistedItem;
            set => SetProperty(ref _isExistedItem, value);
        }

        private ItemModel _selectedItem;
        public ItemModel SelectedItemTapped
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        #endregion

        #region SelectedItemExcute
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

        private async void SelectedItemExcutWithoutPara()
        {
            await CheckBusy(async () =>
            {
                if (SelectedItemTapped.Id != -1)
                {
                    var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == SelectedItemTapped.Id);
                    //await MessagePopup.Instance.Show(SelectedItemTapped.Id.ToString());
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
