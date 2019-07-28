using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Enums;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class CategoryPageViewModel : ViewModelBase
    {
        #region constructor
        public CategoryPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService)
            : base(navigationService: navigationService, sqliteService: sqLiteService)
        {

            IsSearchCommand = new DelegateCommand(IsSearchChangeExcute);
            SelectedCategoryCommand = new DelegateCommand(SelectedCategoryExcute);

            var list = SqLiteService.GetList<Category>(x => x.Id != -1).ToList();
            if (list.Count > 0)
            {
                CategoryList = new ObservableCollection<Category>(list);
            }
            else
            {
                CategoryList = new ObservableCollection<Category>() {
                    new Category()
                {
                        Id=1,
                    CategoryName = TranslateExtension.Get("ElectronicsAndGadgets"),
                    Image = "ic_history",
                },
                new Category()
                {
                    Id=2,
                    CategoryName = TranslateExtension.Get("HomeAndFurniture"),
                    Image = "ic_history",
                },new Category()
                {
                    Id=3,
                    CategoryName = TranslateExtension.Get("SportAndHealth"),
                    Image = "ic_history",
                },new Category()
                {
                    Id=4,
                    CategoryName = TranslateExtension.Get("Fashion"),
                    Image = "ic_history",
                },new Category()
                {
                    Id=5,
                    CategoryName = TranslateExtension.Get("OfficeAndIndustry"),
                    Image = "ic_history",
                }};
                foreach (var item in CategoryList)
                {
                    SqLiteService.Insert(item);
                }
            }
        }
        #endregion

        #region Properties
        private ObservableCollection<Category> _categoryList;

        public ObservableCollection<Category> CategoryList
        {
            get => _categoryList;
            set => SetProperty(ref _categoryList, value);
        }

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

        private ItemModel _selectedItem;
        public ItemModel SelectedItemTapped
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        #endregion

        #region OnFirstTimeAppear
        public override void OnAppear()
        {
            
        }

        #endregion

        #region Override Navigate new to

       

        #endregion

        #region IsSearchCommand
        public ICommand IsSearchCommand { get; set; }
        private void IsSearchChangeExcute()
        {
            IsSearch = true;
        }

        #endregion

        #region SelectedCategoryCommand
        public ICommand SelectedCategoryCommand { get; set; }
        private async void SelectedCategoryExcute()
        {
            await CheckBusy(async () =>
            {
                if (SelectedCategory != null)
                {
                    var param = new NavigationParameters
                {
                    {ParamKey.CategoryId.ToString(), SelectedCategory.Id},
                    {ParamKey.CategoryName.ToString(), SelectedCategory.CategoryName},
                    //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                };

                    await Navigation.NavigateAsync(PageManager.CategoryDetailsPage, parameters: param);
                }
            });

        }
        #endregion

        #region BackCommand

        protected async override Task BackExecute()
        {
            await CheckBusy(async () =>
            {
                IsSearch = false;
            });
        }

        public override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
        #endregion

        

    }
}
