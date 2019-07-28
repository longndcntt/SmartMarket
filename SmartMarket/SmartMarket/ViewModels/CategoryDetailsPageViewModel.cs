using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Enums;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
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
    public class CategoryDetailsPageViewModel : ViewModelBase
    {
        #region Constructor
        public CategoryDetailsPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
           : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest)
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

        private int _selectedCategoryId;
        public int SelectedCategoryId
        {
            get => _selectedCategoryId;
            set => SetProperty(ref _selectedCategoryId, value);
        }


        private string _selectedCategory;
        public string CategoryName
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private ObservableCollection<ItemModel> _itemModelRandomList;
        public ObservableCollection<ItemModel> ItemModelRandomList
        {
            get => _itemModelRandomList;
            set => SetProperty(ref _itemModelRandomList, value);
        }

        private ObservableCollection<ItemModel> _allItemModelList;
        public ObservableCollection<ItemModel> AllItemModelList
        {
            get => _allItemModelList;
            set => SetProperty(ref _allItemModelList, value);
        }


        private ItemModel _selectedItem;
        public ItemModel SelectedItemTapped
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        #endregion

        #region Override Navigate new to

        public async override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.CategoryId.ToString()))
                {
                    SelectedCategoryId = (int)parameters[ParamKey.CategoryId.ToString()];
                    CategoryName = parameters[ParamKey.CategoryName.ToString()].ToString();
                    if (SelectedCategoryId != -1)
                    {
                        var urlRandom = ApiUrl.GetRandomItems();
                        var urlAllCategory = ApiUrl.GetItemByCategory(SelectedCategoryId.ToString());
                        var randomItem = await LoadData(urlRandom);
                        var allItem = await LoadData(urlAllCategory);
                        if (randomItem.Any())
                        {
                            ItemModelRandomList = new ObservableCollection<ItemModel>(randomItem);
                        }
                        if (allItem.Any())
                        {
                            AllItemModelList = new ObservableCollection<ItemModel>(allItem);
                        }
                        //var listTemp = SqLiteService.GetList<ItemModel>(x => x.CategoryId == SelectedCategory.Id).ToList();
                        //if (listTemp.Count > 0)
                        //{
                        //    ItemModelList = new ObservableCollection<ItemModel>(listTemp);
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < 6; i++)
                        //    {
                        //        var item = new ItemModel()
                        //        {
                        //            Id = i,
                        //            ItemName = "SP" + i,
                        //            CategoryId = SelectedCategory.Id,
                        //            //Price = 200 + i * 100,
                        //            Image = "sony_product"
                        //        };
                        //        SqLiteService.Insert(item);
                        //    }
                        //}
                    }
                }
            }
        }

        #endregion

        #region LoadData

        public async Task<IEnumerable<ItemModel>> LoadData(string url)
        {
            try
            {
                IsBusyLoading = true;
                await Task.Delay(2000);
                var a = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
                if (a != null)
                {
                    var listTemp = a.Deserialize<IEnumerable<ItemModel>>(a.Result).ToList();
                    return listTemp;
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            finally
            {
                IsBusyLoading = false;
            }
            return null;
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
                    var iteNumberId = Int32.Parse(itemId);
                    //var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == iteNumberId);
                    //var selectedCategory = SqLiteService.Get<Category>(x => x.Id == selectedItem.CategoryId);
                    var param = new NavigationParameters
                    {
                        {ParamKey.SelectedItemId.ToString(), iteNumberId},
                           // {ParamKey.Category.ToString(), selectedCategory},
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
                    //var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == SelectedItemTapped.Id);
                    ////await MessagePopup.Instance.Show(SelectedItemTapped.Id.ToString());
                    //var selectedCategory = SqLiteService.Get<Category>(x => x.Id == selectedItem.CategoryId);
                    var param = new NavigationParameters
                        {
                            {ParamKey.SelectedItem.ToString(), SelectedItemTapped},
                           // {ParamKey.Category.ToString(), selectedCategory},
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
