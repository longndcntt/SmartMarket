using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class WalletBalancePageViewModel : ViewModelBase
    {
        #region Constructor
        public WalletBalancePageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
      : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {
            AddMoneyCommand = new DelegateCommand(AddMoneyExcute);
            AmountCoin = 100;
        }

        #endregion

        #region Properties
        private double _amountMoney;
        public double AmountMoney
        {
            get => _amountMoney;
            set
            {
                SetProperty(ref _amountMoney, value);
            }
        }

        private double _amountCoin = 100;
        public double AmountCoin
        {
            get => _amountCoin;
            set
            {
                SetProperty(ref _amountCoin, value);
                AmountMoney = AmountCoin * 1000;
            }
        }
        #endregion

        #region OnAppear
        public async override void OnAppear()
        {
            await CheckBusy(async () =>
            {
                await LoadingPopup.Instance.Show(TranslateExtension.Get("Loading3dot"));

                await Task.Run(async () =>
                {
                    var url = ApiUrl.GetWallet() + "0x262036d0E87D7fA0a201cF2443aA3450dE388b4b";

                    var response = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
                    await GetWalletCallBack(response);
                });

            });

        }

        private async Task GetWalletCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show("Fail");
            }
            else
            {
                Serializer serializer = new Serializer();
                var a = response.Result.ToString();
                var b = serializer.Deserialize<ModelTestApi>(a);
                if (b != null)
                {
                    AmountCoin = b.Amount;
                }
            }
            await LoadingPopup.Instance.Hide();
        }

        #endregion

        #region AddMoneyCommand
        public ICommand AddMoneyCommand { get; set; }
        private async void AddMoneyExcute()
        {
            var param = new NavigationParameters
                {
                    {ParamKey.CoinInWallet.ToString(), AmountCoin},
                    //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                };
            await Navigation.NavigateAsync(PageManager.AddMoneyPage, param);
        }
        #endregion
    }
}
