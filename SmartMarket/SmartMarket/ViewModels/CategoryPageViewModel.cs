﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Enums;
using SmartMarket.Interfaces.LocalDatabase;
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
                    CategoryName = "Electronics and Gadgets",
                    Image = "ic_history",
                },
                new Category()
                {
                    Id=2,
                    CategoryName = "Home and Furniture",
                    Image = "ic_history",
                },new Category()
                {
                    Id=3,
                    CategoryName = "Sport and Health",
                    Image = "ic_history",
                },new Category()
                {
                    Id=4,
                    CategoryName = "Fashion",
                    Image = "ic_history",
                },new Category()
                {
                    Id=5,
                    CategoryName = "Office and Industry",
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
                    {ParamKey.Category.ToString(), SelectedCategory},
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
