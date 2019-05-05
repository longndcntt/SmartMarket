using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Enums;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
using SmartMarket.ViewModels.Base;
using SmartMarket.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class ItemDetailsPageViewModel : ViewModelBase
    {
        #region constructor

        #endregion
        public ItemDetailsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ISqLiteService sqLiteService)
            : base(navigationService: navigationService, dialogService: pageDialogService, sqliteService: sqLiteService)
        {
            ShowMoreItemDetailCommand = new DelegateCommand(ShowMoreItemDetailExcute);
            NavigateToCartCommand = new DelegateCommand(NavigateToCartExcute);
            AddToCartCommand = new DelegateCommand(AddToCartExcute);
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
        private int _position = 0;
        public int Position
        {
            get => _position;
            set
            {
                SetProperty(ref _position, value);
            }
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
        public override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.Category.ToString()))
                {
                    SelectedCategory = (Category)parameters[ParamKey.Category.ToString()];
                    ItemSelected = (ItemModel)parameters[ParamKey.SelectedItem.ToString()];
                }
                if (ItemSelected != null)
                {
                    ItemSelectedDetails = new ItemDetails()
                    {
                        Count = 5,
                        Manufacturer = "Japan",
                        ProductId = ItemSelected.Id,
                        Detail = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                        " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                        " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen",
                    };

                    ItemSelectedImage = new ObservableCollection<ImageItemDetail>()
                    {
                        new ImageItemDetail()
                        {
                            ProductID = ItemSelected.Id,
                            Image = "sony_product"
                        },
                        new ImageItemDetail()
                        {
                            ProductID = ItemSelected.Id,
                            Image = "sony_product"
                        },
                        new ImageItemDetail()
                        {
                            ProductID = ItemSelected.Id,
                            Image = "sony_product"
                        }
                    };

                    ReviewProductList = new ObservableCollection<ReviewProduct>()
                    {
                        new ReviewProduct()
                        {
                            ProductId = ItemSelected.Id,
                            Rate = 3,
                            ReviewedDateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                            Content = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                        " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen.",
                        },
                        new ReviewProduct()
                        {
                            ProductId = ItemSelected.Id,
                            Rate = 5,
                            ReviewedDateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                            Content = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                        " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen.",
                        },
                        new ReviewProduct()
                        {
                            ProductId = ItemSelected.Id,
                            Rate = 4,
                            ReviewedDateTime = DateTime.Now.ToString("yyyy-MM-dd"),
                            Content = "Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen." +
                        " Favorite Channel List, Parental Control Function, Auto Tuning, Closed Caption, Auto, 16:9 Pillar Box, 16:9 Pan G Scan, 4:3 Letter Box, 4:3 Pan G Scan, 4:3 Full, 16:9 Wide Screen.",
                        }
                    };
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
        public ICommand AddToCartCommand { get; set; }
        private void AddToCartExcute()
        {
        }

        #endregion

        #region MyRegion
        public ICommand NavigateToCartCommand { get; set; }
        private void NavigateToCartExcute()
        {
        }
        #endregion

    }
}
