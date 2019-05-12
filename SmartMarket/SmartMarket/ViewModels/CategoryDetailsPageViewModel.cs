using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Enums;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
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
    public class CategoryDetailsPageViewModel : ViewModelBase
    {
        #region Constructor
        public CategoryDetailsPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService)
           : base(navigationService: navigationService, sqliteService: sqLiteService)
        {
            IsSearchCommand = new DelegateCommand(IsSearchChangeExcute);
            ItemTappedCommand = new DelegateCommand(SelectedItemExcutWithoutPara);

        }

      
        #endregion

        #region Properties

        private bool _isSearch;
        public bool IsSearch
        {
            get => _isSearch;
            set => SetProperty(ref _isSearch, value);
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private ObservableCollection<ItemModel> _itemModelList;
        public ObservableCollection<ItemModel> ItemModelList
        {
            get => _itemModelList;
            set => SetProperty(ref _itemModelList, value);
        }

        private ItemModel _selectedItem;
        public ItemModel SelectedItemTapped
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        #endregion

        #region Override Navigate new to

        public override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.Category.ToString()))
                {
                    SelectedCategory = (Category)parameters[ParamKey.Category.ToString()];
                    if (SelectedCategory != null)
                    {
                        var listTemp = SqLiteService.GetList<ItemModel>(x => x.CategoryId == SelectedCategory.Id).ToList();


                        if (listTemp.Count > 0)
                        {
                            ItemModelList = new ObservableCollection<ItemModel>(listTemp);
                        }
                        else
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                var item = new ItemModel()
                                {
                                    Id = i,
                                    ItemName = "SP" + i,
                                    CategoryId = SelectedCategory.Id,
                                    Price = 20000 + i * 10000,
                                    Image = "sony_product"
                                };
                                SqLiteService.Insert(item);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region IsSearchCommand
        public ICommand IsSearchCommand { get; set; }
        private void IsSearchChangeExcute()
        {
            IsSearch = true;
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
                    var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == Int32.Parse(itemId));
                    var param = new NavigationParameters
                {
                    {ParamKey.SelectedItem.ToString(), selectedItem},
                            {ParamKey.Category.ToString(), SelectedCategory},
                    //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                };

                    await Navigation.NavigateAsync(PageManager.CategoryDetailsPage, parameters: param);
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
                     var param = new NavigationParameters
                        {
                            {ParamKey.SelectedItem.ToString(), selectedItem},
                            {ParamKey.Category.ToString(), SelectedCategory},
                         //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                        };

                     await Navigation.NavigateAsync(PageManager.ItemDetailsPage, parameters: param);
                 }
             });
        }
        #endregion

        #region BackCommand

        protected async override Task BackExecute()
        {
            if (IsSearch)
            {
                IsSearch = false;
            }
            else
            {
                await base.BackExecute();
            }

        }
        #endregion
    }
}
