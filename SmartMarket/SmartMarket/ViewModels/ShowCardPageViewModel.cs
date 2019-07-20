using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
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
using Xamarin.Forms;

namespace SmartMarket.ViewModels
{
    public class ShowCardPageViewModel : ViewModelBase
    {
        #region Constructor
        public ShowCardPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
      : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest)
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
            NavigateToCheckoutCommand = new DelegateCommand(NavigateToCheckoutExcute);

        }

      
        #endregion

        #region Properties
        private double _total;
        public double Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }
        #endregion

        #region OnAppear
        public override void OnAppear()
        {
            base.OnAppear();
        }
        #endregion

        #region DeleteItem

        public void DeleteItemOfOrder(string id)
        {
            var listItemOfCartTemp = SqLiteService.GetList<OrderDetails>(x => x.Id != "");
            ItemOfOrder = new ObservableCollection<OrderDetails>();
            if (listItemOfCartTemp != null)
            {
                ItemOfOrder = new ObservableCollection<OrderDetails>(listItemOfCartTemp);
            }
            foreach (var item in ItemOfOrder.ToList())
            {
                if (item.Id == id)
                {
                    ItemOfOrder.Remove(item);
                    SqLiteService.DeleteAll<OrderDetails>();
                    SqLiteService.InsertAll<OrderDetails>(ItemOfOrder);
                    Total -= item.Amount * item.Price;
                }
            }
        }

        #endregion

        #region EditQuantity

        public void EditQuantityItemOfOrder(string id, int amount)
        {
            var listItemOfCartTemp = SqLiteService.GetList<OrderDetails>(x => x.Id != "");
            ItemOfOrder = new ObservableCollection<OrderDetails>();
            if (listItemOfCartTemp != null)
            {
                ItemOfOrder = new ObservableCollection<OrderDetails>(listItemOfCartTemp);
            }
            foreach (var item in ItemOfOrder.ToList())
            {
                if (item.Id == id)
                {
                    if (amount > item.Amount)
                    {
                        Total += (amount - item.Amount) * item.Price;
                    }
                    else
                    {
                        Total -= (item.Amount - amount) * item.Price;
                    }
                    item.Amount = amount;
                    SqLiteService.DeleteAll<OrderDetails>();
                    SqLiteService.InsertAll<OrderDetails>(ItemOfOrder);
                }
            }
        }
        #endregion

        #region NavigateToCheckoutCommand
        public ICommand NavigateToCheckoutCommand { get; set; }
        private async void NavigateToCheckoutExcute()
        {
            if (!App.Settings.IsLogin)
            {
                await Navigation.NavigateAsync(PageManager.LoginSignUpTabbedPage);
                return;
            }
            await Navigation.NavigateAsync(PageManager.ProceedToCheckoutPage);
        }

        #endregion
    }
}
