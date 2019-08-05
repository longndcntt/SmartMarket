using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Files;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.ViewModels.Base;
using System;
using Microcharts;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using SmartMarket.Models;
using SmartMarket.Views.Popups;
using SmartMarket.Localization;
using SmartMarket.Services.HttpService;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using Entry = Microcharts.Entry;

namespace SmartMarket.ViewModels
{
	public class StatisticPageViewModel : ViewModelBase
	{
        #region Properties
        private ObservableCollection<ExchangeModel> _puchasedItemList;
        public ObservableCollection<ExchangeModel> PuchasedItemList
        {
            get => _puchasedItemList;
            set => SetProperty(ref _puchasedItemList, value);
        }

        private ObservableCollection<UserExchange> _userExchangeList;
        public ObservableCollection<UserExchange> UserExchangeList
        {
            get => _userExchangeList;
            set => SetProperty(ref _userExchangeList, value);
        }
        
        private DonutChart _chart;
        public DonutChart Chart
        {
            get => _chart;
            set => SetProperty(ref _chart, value);
        }

        private double _Total;
        public double Total
        {
            get => _Total;
            set => SetProperty(ref _Total, value);
        }

        
        #endregion

        public StatisticPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService,
            IHttpRequest httpRequest, IPageDialogService dialogService)
        : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest, dialogService: dialogService)
        {
            UserInfo = SqLiteService.Get<UserModel>(x => x.Id != -1);
        }

        public async override void OnAppear()
        {
            base.OnAppear();
            if (!string.IsNullOrEmpty(UserInfo.WalletAddress))
            {
                await LoadingPopup.Instance.Show(TranslateExtension.Get("Loading3dot"));
                var url = ApiUrl.GetSelledItems(UserInfo.WalletAddress);

                var response = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
                await GetWalletCallBack(response);
            }
        }

        private async Task GetWalletCallBack(ModelRestFul response)
        {
            if (response == null)
            {
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
                return;
            }
            else
            {
                Serializer serializer = new Serializer();
                var a = response.Result.ToString();
                var b = serializer.Deserialize<ObservableCollection<ExchangeModel>>(a);
                if (b != null)
                {
                    PuchasedItemList = new ObservableCollection<ExchangeModel>(b);
                    foreach (var item in PuchasedItemList)
                    {
                        item.StatusExchange = (item.remain > 0) ? (item.isDone) ? Color.Red : Color.Orange : Color.LightGreen;
                        item.StatusExchangeString = (item.remain > 0) ? (item.isDone) ? TranslateExtension.Get("PaymentFail") : TranslateExtension.Get("PaymentPending") : TranslateExtension.Get("Success");
                    }
                    LoadChart();
                    var listUser = PuchasedItemList.GroupBy(x=>x.buyer.FullName);
                    if (listUser !=null)
                    {

                        UserExchangeList = new ObservableCollection<UserExchange>();
                        foreach (var item in listUser)
                        {
                            var user = new UserExchange()
                            {
                                FullName = item.Key,
                                TotalPrice = string.Format("{0:0,0}", PuchasedItemList.Where(x => x.buyer.FullName == item.Key && x.remain == 0 && x.isDone==true ).Sum(x => x.productInfo.Price)),
                            };
                            UserExchangeList.Add(user);
                        }
                        ;
                        UserExchangeList = new ObservableCollection<UserExchange>(UserExchangeList.OrderBy(x => x.TotalPrice));
                    }
                }
            }
            await LoadingPopup.Instance.Hide();
        }

        private void LoadChart()
        {
            if (PuchasedItemList != null)
            {
                double Total1 = 0;
                double Total2 = 0;
                double Total3 = 0;
                double Total4 = 0;
                double Total5 = 0;
                foreach (var item in PuchasedItemList)
                {
                    if (item.StatusExchange == Color.LightGreen)
                    {
                        switch (item.productInfo.CategoryId)
                        {
                            case 1:
                                Total1 += item.productInfo.Price;
                                break;
                            case 2:
                                Total2 += item.productInfo.Price;
                                break;
                            case 3:
                                Total3 += item.productInfo.Price;
                                break;
                            case 4:
                                Total4 += item.productInfo.Price;
                                break;
                            case 5:
                                Total5 += item.productInfo.Price;
                                break;
                        }
                    }
                }
                Total = Total1 + Total2 + Total3 + Total4 + Total5;
                var entries = new[]
                {
                    new Microcharts.Entry((float)Total1)
                    {
                         Label = TranslateExtension.Get("ElectronicsAndGadgets"),
                        ValueLabel = string.Format("{0:0,0}", Total1),
                        Color = SKColor.Parse("#005900")
                    },
                    new Entry((float)Total2)
                    {
                         Label = TranslateExtension.Get("HomeAndFurniture"),
                        ValueLabel = string.Format("{0:0,0}", Total2),
                        Color = SKColor.Parse("#e00707")
                    },
                    new Entry((float)Total3)
                    {
                         Label = TranslateExtension.Get("SportAndHealth"),
                        ValueLabel = string.Format("{0:0,0}", Total3),
                        Color = SKColor.Parse("#0f3766")
                    },
                    new Entry((float)Total4)
                    {
                         Label = TranslateExtension.Get("Fashion"),
                        ValueLabel = string.Format("{0:0,0}", Total4),
                        Color = SKColor.Parse("#fcb8f8")
                    },
                    new Entry((float)Total5)
                    {
                         Label = TranslateExtension.Get("OfficeAndIndustry"),
                        ValueLabel = string.Format("{0:0,0}", Total5),
                        Color = SKColor.Parse("#04ecf0")
                    }
                };
                Chart = new DonutChart();
                Chart.Entries = entries.Where(x => x.Value > 0);
                Chart.IsAnimated = true;
                Chart.AnimationProgress = 2;
                Chart.LabelTextSize = 26;
                Chart.BackgroundColor = SKColor.Parse("#fefefe");
            }
        }
    }

    public class UserExchange
    {
        public string FullName { get; set; }
        public string TotalPrice { get; set; }
    }
}
