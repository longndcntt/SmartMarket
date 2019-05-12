using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartMarket.ViewModels
{
    public class ProceedToCheckoutPageViewModel : ViewModelBase
    {
        #region constructor
        public ProceedToCheckoutPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ISqLiteService sqLiteService)
            : base(navigationService: navigationService, dialogService: pageDialogService, sqliteService: sqLiteService)
        {
            if (!IsNullCart)
            {
                var listItemOfCartTemp = SqLiteService.GetList<OrderDetails>(x => x.Id != "");
                ItemOfOrder = new ObservableCollection<OrderDetails>();
                if (listItemOfCartTemp != null)
                {
                    ItemOfOrder = new ObservableCollection<OrderDetails>(listItemOfCartTemp);
                    foreach (var item in ItemOfOrder)
                    {
                        Total += item.Amount * item.Price;
                    }
                }
            }
            ChooseShippingList = new ObservableCollection<string>()
            {
                TranslateExtension.Get("FreeShip"),
                TranslateExtension.Get("Standard"),
                TranslateExtension.Get("Express"),
            };
            // NavigateToCheckoutCommand = new DelegateCommand(NavigateToCheckoutExcute);
        }
        #endregion

        #region OnAppearing
        public override void OnAppear()
        {
            ChooseShippingList = new ObservableCollection<string>()
            {
                TranslateExtension.Get("FreeShip"),
                TranslateExtension.Get("Standard"),
                TranslateExtension.Get("Express"),
            };

        }
        #endregion
        #region Properties
        private double _total;

        public double Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private double _subTotal;

        public double SubTotal
        {
            get => _subTotal;
            set => SetProperty(ref _subTotal, value);
        }

        private ObservableCollection<string> _chooseShippingList;
        public ObservableCollection<string> ChooseShippingList
        {
            get => _chooseShippingList;
            set => SetProperty(ref _chooseShippingList, value);
        }

        private double shipping;
        private int _selectedShipping =0;
        public int SelectedShipping
        {
            get => _selectedShipping;
            set
            {
                SetProperty(ref _selectedShipping, value);
                if (SelectedShipping != -1)
                {
                    switch (SelectedShipping)
                    {
                        case 1:
                            shipping = 10000;
                            SubTotal = Total + shipping;
                            break;
                        case 2:
                            shipping = 20000;
                            SubTotal = Total + shipping;
                            break;
                        default:
                            SubTotal = Total;
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
