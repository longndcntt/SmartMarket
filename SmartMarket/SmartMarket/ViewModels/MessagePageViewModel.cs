using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Enums;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
	public class MessagePageViewModel : ViewModelBase
	{
        public MessagePageViewModel(INavigationService navigationService, ISqLiteService sqLiteService) : base(navigationService: navigationService, sqliteService: sqLiteService)
        {
            NavigatieMainpageCommand = new DelegateCommand(NavigatieMainpageExcute);
        }

        

        #region Properties

        private double _addAmountCoin;
        public double AddAmountCoin
        {
            get => _addAmountCoin;
            set
            {
                SetProperty(ref _addAmountCoin, value);

            }
        }

        private double _addAmountMoney;
        public double AddAmountMoney
        {
            get => _addAmountMoney;
            set
            {
                SetProperty(ref _addAmountMoney, value);

            }
        }

        private string _transactionID;
        public string TransactionID
        {
            get => _transactionID;
            set
            {
                SetProperty(ref _transactionID, value);

            }
        }
        #endregion
        #region OnAppear
        public async override void OnAppear()
        {
            base.OnAppear();
            SqLiteService.Update(App.Settings);
            await Task.Delay(3000);
            await Navigation.GoBackToRootAsync();
        }
        #endregion
        #region OnNavigate
        public override void OnNavigatedNewToAsync(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.CoinInWallet.ToString()))
                {
                    AddAmountCoin = (double)parameters[ParamKey.CoinInWallet.ToString()];
                }
                if (parameters.ContainsKey(ParamKey.TransactionID.ToString()))
                {
                    TransactionID = (string)parameters[ParamKey.TransactionID.ToString()];
                }
            }
        }
        #endregion

        #region NavigatieMainpageCommand
        public ICommand NavigatieMainpageCommand { get; set; }
        private async void NavigatieMainpageExcute()
        {
            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                await Navigation.GoBackToRootAsync();
            });
        }
        #endregion
    }
}
