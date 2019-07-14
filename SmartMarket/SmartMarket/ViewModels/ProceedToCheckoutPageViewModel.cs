using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
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
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class ProceedToCheckoutPageViewModel : ViewModelBase
    {
        #region constructor
        public ProceedToCheckoutPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
            : base(navigationService: navigationService, dialogService: pageDialogService, sqliteService: sqLiteService, httpRequest: httpRequest)
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
            CheckoutCommand = new DelegateCommand(CheckoutExcute);

            if (App.Settings.IsLogin)
            {
                UserInfo = SqLiteService.Get<UserModel>(x => x.Id != 0);
            }
        }
        #endregion

        #region OnAppearing
        public async override void OnAppear()
        {
            base.OnAppear();
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
        private int _selectedShipping = 0;
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

        private double _amountCoin = 100;
        public double AmountCoin
        {
            get => _amountCoin;
            set
            {
                SetProperty(ref _amountCoin, value);
            }
        }
        #endregion

        #region Checkout
        public ICommand CheckoutCommand { get; set; }
        private async void CheckoutExcute()
        {

            await LoadingPopup.Instance.Show(TranslateExtension.Get("Loading3dot"));

            await Task.Run(async () =>
            {
                var url = ApiUrl.Schedule();
                var buyItem = new BuyItem()
                {
                    AddressFrom = UserInfo.WalletAddress,
                    AddressTo = "0xd5488AD7D33c7d82414c50365C1FaF2081648D9d",
                    Amount = new double[] { SubTotal / 2, SubTotal / 2 },
                    Time = new int[] { 20, 50 },
                };
                //string sData = Newtonsoft.Json.JsonConvert.SerializeObject(buyItem);
                //var httpContent = new StringContent(sData, System.Text.Encoding.UTF8);
                //httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var httpContent = buyItem.ObjectToStringContent();
                var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
                await AddCoinCallBack(response);
            });
        }

        private async Task AddCoinCallBack(ModelRestFul response)
        {

            if (response == null)
            {
                await MessagePopup.Instance.Show("Fail");
            }
            else
            {
                var transaction = response.Deserialize<Transaction>(response.Result);
                if (transaction != null)
                {
                    var signer = new Signer();
                    var privatekey = UserInfo.PrivateKey;
                    var stringSigned = signer.Sign(privatekey, transaction);
                    //var transactionID = transaction.Transaction;
                    if (!string.IsNullOrEmpty(stringSigned))
                    {
                        await UploadToBlockchain(stringSigned);
                    }
                }
            }
        }

        private async Task UploadToBlockchain(string stringSigned)
        {
            var url = ApiUrl.UploadToBlockChain();
            var signed = new SignedTransaction(stringSigned);
            var httpContent = signed.ObjectToStringContent();
            var response = await HttpRequest.PostTaskAsync<ModelRestFul>(url, httpContent);
            await UploadToBlockchainCallBack(response);
        }

        private async Task UploadToBlockchainCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show("Fail");
                // get event list fail
                //await MessagePopup.Instance.Show(TranslateExtension.Get("GetListEventsFailed"));
            }
            else
            {
                // get event list successfull
                var transaction = response.Deserialize<TransactionIDModel>(response.Result);
                if (transaction != null)
                {
                    var transactionID = transaction.TransactionID;
                    await LoadingPopup.Instance.Hide();
                    var param = new NavigationParameters
                {
                    {ParamKey.CoinInWallet.ToString(), SubTotal},
                    {ParamKey.TransactionID.ToString(), transactionID},
                    //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                };
                    await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                    {
                        await Navigation.NavigateAsync(PageManager.MessagePage, param);
                    });

                }
            }
            await LoadingPopup.Instance.Hide();
        }


        private async void NavigateToMessageExcute()
        {
            await Navigation.NavigateAsync(PageManager.MessagePage);
        }
        #endregion
    }
}
