using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Services;
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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Constructor
        public ICommand CheckCartCommand { get; set; }
        private int _position = 0;
        public int Position
        {
            get => _position;
            set
            {
                SetProperty(ref _position, value);
            }
        }
        private ObservableCollection<ItemModel> _myList;

        public ObservableCollection<ItemModel> ListRandom
        {
            get => _myList;
            set => SetProperty(ref _myList, value);
        }

        private ObservableCollection<ItemModel> _listItem1;

        public ObservableCollection<ItemModel> ListItem1
        {
            get => _listItem1;
            set => SetProperty(ref _listItem1, value);
        }

        private ObservableCollection<ItemModel> _listItem2;

        public ObservableCollection<ItemModel> ListItem2
        {
            get => _listItem2;
            set => SetProperty(ref _listItem2, value);
        }

        private ObservableCollection<ItemModel> _listItem3;

        public ObservableCollection<ItemModel> ListItem3
        {
            get => _listItem3;
            set => SetProperty(ref _listItem3, value);
        }

        private ObservableCollection<ItemModel> _listItem4;

        public ObservableCollection<ItemModel> ListItem4
        {
            get => _listItem4;
            set => SetProperty(ref _listItem4, value);
        }

        private ObservableCollection<ItemModel> _listItem5;

        public ObservableCollection<ItemModel> ListItem5
        {
            get => _listItem5;
            set => SetProperty(ref _listItem5, value);
        }

        private ObservableCollection<ItemModel> _listItem6;

        public ObservableCollection<ItemModel> ListItem6
        {
            get => _listItem6;
            set => SetProperty(ref _listItem6, value);
        }

        private ItemModel _selectedItem;
        public ItemModel SelectedItemTapped
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private ObservableCollection<string> _listEvent;
        public ObservableCollection<string> ListEvent
        {
            get => _listEvent;
            set => SetProperty(ref _listEvent, value);
        }
        #endregion
        public MainPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest, IPageDialogService pageDialogService)
           : base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest, dialogService: pageDialogService)
        {
            //Check Cart
            var order = SqLiteService.Get<Order>(x => x.Id == 0);
            if (order == null)
            {
                IsNullCart = false;
                OrderOfUser = new Order();
                ItemOfOrder = new ObservableCollection<OrderDetails>();
                SqLiteService.Insert(OrderOfUser);
            }
            else
            {
                var listItemOfCartTemp = SqLiteService.GetList<OrderDetails>(x => x.Id != "");
                ItemOfOrder = new ObservableCollection<OrderDetails>();
                if (listItemOfCartTemp != null)
                {
                    ItemOfOrder = new ObservableCollection<OrderDetails>(listItemOfCartTemp);
                }

            }
            //

            CheckCartCommand = new DelegateCommand(NavigateShowCardPage);
            LoadingAgainCommand = new DelegateCommand(LoadDataAgain);
            Title = TranslateExtension.Get("MainPage");
            ItemTappedCommand = new DelegateCommand(SelectedItemExcutWithoutPara);
            //MyList = new ObservableCollection<ItemModel>()
            //{
            //    new ItemModel()
            //    {
            //        ItemName = "But Long1",
            //        Price = 20000,
            //        Image= "https://a.imge.to/2019/07/15/F9MQt.th.png",
            //    },
            //    new ItemModel()
            //    {
            //        ItemName = "But Long2",
            //        Price = 20000,
            //        Image= "https://a.imge.to/2019/07/15/F9MQt.th.png"
            //    },
            //    new ItemModel()
            //    {
            //        ItemName = "But Long3",
            //        Price = 20000,
            //        Image= "https://a.imge.to/2019/07/15/F9MQt.th.png"
            //    },
            //    new ItemModel()
            //    {
            //        ItemName = "But Long4",
            //        Price = 20000,
            //        Image= "https://a.imge.to/2019/07/15/F9MQt.th.png"
            //    },
            //    new ItemModel()
            //    {
            //        ItemName = "But Long5",
            //        Price = 20000,
            //        Image= "https://a.imge.to/2019/07/15/F9MQt.th.png"
            //    },
            //    new ItemModel()
            //    {
            //        Id = 4,
            //        ItemName = "But Long6",
            //        Price = 20000,
            //        Image= "https://a.imge.to/2019/07/15/F9MQt.th.png"
            //    },
            //};

            ListEvent = new ObservableCollection<string>()
            {
                "one.png","two.jpg","one.png","two.jpg"
            };

        }


        public async override void OnAppear()
        {
            try
            {
                var tempRandomAll = await LoadData(ApiUrl.GetRandomItems());
                ListRandom = new ObservableCollection<ItemModel>(tempRandomAll);
                var temp2 = await LoadData(ApiUrl.GetItemByCategory("1"));
                ListItem1 = new ObservableCollection<ItemModel>(temp2);
                var temp3 = await LoadData(ApiUrl.GetItemByCategory("2"));
                ListItem2 = new ObservableCollection<ItemModel>(temp3);
                var temp4 = await LoadData(ApiUrl.GetItemByCategory("3"));
                ListItem3 = new ObservableCollection<ItemModel>(temp4);
                var temp5 = await LoadData(ApiUrl.GetItemByCategory("4"));
                ListItem4 = new ObservableCollection<ItemModel>(temp5);
                var temp6 = await LoadData(ApiUrl.GetItemByCategory("5"));
                ListItem5 = new ObservableCollection<ItemModel>(temp6);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
                return;
            }
          
        }

        public async Task<IEnumerable<ItemModel>> LoadData(string url)
        {
            if (IsBusyLoading)
                return null;
            try
            {
                IsBusyLoading = true;
                await Task.Delay(2000);
                var a = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
                if (a != null)
                {
                    var listTemp = a.Deserialize<IEnumerable<ItemModel>>(a.Result).ToList();
                    return listTemp;
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            finally
            {
                IsBusyLoading = false;
            }
            return null;
        }

        private async void ItemTappedExcute()
        {
            await MessagePopup.Instance.Show(SelectedItemTapped.ItemName);
            // await Navigation.NavigateAsync(PageManager.LoginPage);
        }

        #region SelectedItemExcute
        public ICommand ItemTappedCommand { get; set; }
        public async void SelectedItemExcute(string itemId)
        {
            await CheckBusy(async () =>
            {
                if (!string.IsNullOrEmpty(itemId))
                {
                    var iteNumberId = Int32.Parse(itemId);
                    //var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == iteNumberId);
                    //var selectedCategory = SqLiteService.Get<Category>(x => x.Id == selectedItem.CategoryId);
                    var param = new NavigationParameters
                    {
                        {ParamKey.SelectedItemId.ToString(), iteNumberId},
                           // {ParamKey.Category.ToString(), selectedCategory},
                        //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                    };

                    await Navigation.NavigateAsync(PageManager.ItemDetailsPage, parameters: param);
                }
            });
        }

        private async void SelectedItemExcutWithoutPara()
        {
            await CheckBusy(async () =>
            {
                if (SelectedItemTapped.Id != -1)
                {
                    //var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == SelectedItemTapped.Id);
                    ////await MessagePopup.Instance.Show(SelectedItemTapped.Id.ToString());
                    //var selectedCategory = SqLiteService.Get<Category>(x => x.Id == selectedItem.CategoryId);
                    var param = new NavigationParameters
                        {
                            {ParamKey.SelectedItem.ToString(), SelectedItemTapped},
                           // {ParamKey.Category.ToString(), selectedCategory},
                        //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                    };

                    await Navigation.NavigateAsync(PageManager.ItemDetailsPage, parameters: param);
                }
            });
        }
        #endregion

        #region LoadingAgainCommand
        public ICommand LoadingAgainCommand { get; set; }
        private async void LoadDataAgain()
        {
            try
            {
                var tempRandomAll = await LoadData(ApiUrl.GetRandomItems());
                ListRandom = new ObservableCollection<ItemModel>(tempRandomAll);
                var temp2 = await LoadData(ApiUrl.GetItemByCategory("1"));
                ListItem1 = new ObservableCollection<ItemModel>(temp2);
                var temp3 = await LoadData(ApiUrl.GetItemByCategory("2"));
                ListItem2 = new ObservableCollection<ItemModel>(temp3);
                var temp4 = await LoadData(ApiUrl.GetItemByCategory("3"));
                ListItem3 = new ObservableCollection<ItemModel>(temp4);
                var temp5 = await LoadData(ApiUrl.GetItemByCategory("4"));
                ListItem4 = new ObservableCollection<ItemModel>(temp5);
                var temp6 = await LoadData(ApiUrl.GetItemByCategory("5"));
                ListItem5 = new ObservableCollection<ItemModel>(temp6);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await MessagePopup.Instance.Show(TranslateExtension.Get("Fail"));
                return;
            }
        }
        #endregion
    }
}
