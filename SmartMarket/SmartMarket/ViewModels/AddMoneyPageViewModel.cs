using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Enums;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.Models.API;
using SmartMarket.Services.HttpService;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using SmartMarket.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class AddMoneyPageViewModel : ViewModelBase
    {
        public AddMoneyPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
        : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {
            AddCoinCommand = new DelegateCommand(AddCoinExcute);
            ChooseWayCheckoutList = new ObservableCollection<string>()
            {
                "ATM","Visa/Credit Card","Paypal"
            };
            if (App.Settings.IsLogin)
            {
                UserInfo = SqLiteService.Get<UserModel>(user => user.Id != -1);
            }
        }

        #region Navigate
        public override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            base.OnNavigatedNewToAsync(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.CoinInWallet.ToString()))
                {
                    AmountCoin = (double)parameters[ParamKey.CoinInWallet.ToString()];
                }
            }
        }
        #endregion
        #region Properties
        private double _addAmountMoney;

        public double AddAmountMoney
        {
            get => _addAmountMoney;
            set
            {
                SetProperty(ref _addAmountMoney, value);
                AddAmountCoin = AddAmountMoney / 10;
            }
        }

        private double _addAmountCoin;

        public double AddAmountCoin
        {
            get => _addAmountCoin;
            set => SetProperty(ref _addAmountCoin, value);
        }

        private double _amountCoin;

        public double AmountCoin
        {
            get => _amountCoin;
            set => SetProperty(ref _amountCoin, value);
        }

        private ObservableCollection<string> _chooseWayCheckoutList;
        public ObservableCollection<string> ChooseWayCheckoutList
        {
            get => _chooseWayCheckoutList;
            set => SetProperty(ref _chooseWayCheckoutList, value);
        }

        private double shipping;
        private int _selectedWayCheckout = 0;
        public int SelectedWayCheckout
        {
            get => _selectedWayCheckout;
            set
            {
                SetProperty(ref _selectedWayCheckout, value);
                //if (SelectedShipping != -1)
                //{
                //    switch (SelectedShipping)
                //    {
                //        case 1:
                //            shipping = 10000;
                //            SubTotal = Total + shipping;
                //            break;
                //        case 2:
                //            shipping = 20000;
                //            SubTotal = Total + shipping;
                //            break;
                //        default:
                //            SubTotal = Total;
                //            break;
                //    }
                //}
            }
        }
        #endregion

        #region AddCoinCommand
        public ICommand AddCoinCommand { get; set; }
        private async void AddCoinExcute()
        {

            await LoadingPopup.Instance.Show(TranslateExtension.Get("Loading3dot"));

            await Task.Run(async () =>
            {
                var url = ApiUrl.BuyToken();
                var buyItem = new BuyItemModel()
                {
                    Address = UserInfo.WalletAddress,
                    Amount = AddAmountCoin,
                };
                //string sData = Newtonsoft.Json.JsonConvert.SerializeObject(buyItem);
                //var httpContent = new StringContent(sData, System.Text.Encoding.UTF8);
                //httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var httpContent = buyItem.ObjectToStringContent();
                var response = await HttpRequest.PutTaskAsync<ModelRestFul>(url, httpContent);
                await AddCoinCallBack(response);
            });
        }

        private async Task AddCoinCallBack(ModelRestFul response)
        {

            if (response == null)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
            }
            else
            {
                var transaction = response.Deserialize<TransactionIDModel>(response.Result);
                if (transaction != null)
                {
                    var transactionID = transaction.TransactionID;
                    await LoadingPopup.Instance.Hide();
                    var param = new NavigationParameters
                {
                    {ParamKey.CoinInWallet.ToString(), AddAmountCoin},
                    {ParamKey.TransactionID.ToString(), transactionID},
                    //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                };
                    await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                    {
                        await Navigation.NavigateAsync(PageManager.MessagePage, param);
                    });
                    
                }
                else
                {
                    await LoadingPopup.Instance.Hide();
                }
            }
        }

        #endregion
    }
}
