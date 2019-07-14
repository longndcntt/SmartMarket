using Prism.Commands;
using Prism.Mvvm;
using SmartMarket.Models.API;
using Prism.Navigation;
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
using System.Diagnostics;
using System.Collections.ObjectModel;
using SmartMarket.Enums;
using Prism.Services;

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

        public ObservableCollection<ItemModel> MyList
        {
            get => _myList;
            set => SetProperty(ref _myList, value);
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
            Title = TranslateExtension.Get("MainPage");
            ItemTappedCommand = new DelegateCommand(SelectedItemExcutWithoutPara);


            ListEvent = new ObservableCollection<string>()
            {
                "one.png","two.jpg","one.png","two.jpg"
            };

        }

       

        public async override void OnAppear()
        {
            var url = ApiUrl.GetAllItem();
            var a = await HttpRequest.GetTaskAsync<ModelRestFul>(url);
            if (a != null)
            {
                var listTemp = a.Deserialize<IEnumerable<ItemModel>>(a.Result);
                if (listTemp.Any())
                {
                    MyList = new ObservableCollection<ItemModel>(listTemp);
                    foreach (var item in MyList)
                    {
                        item.Image = "006-notification";
                    }
                }
            }
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
                    var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == iteNumberId);
                    var selectedCategory = SqLiteService.Get<Category>(x => x.Id == selectedItem.CategoryId);
                    var param = new NavigationParameters
                {
                    {ParamKey.SelectedItem.ToString(), selectedItem},
                            {ParamKey.Category.ToString(), selectedCategory},
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
                    var selectedItem = SqLiteService.Get<ItemModel>(x => x.Id == SelectedItemTapped.Id);
                    //await MessagePopup.Instance.Show(SelectedItemTapped.Id.ToString());
                    var selectedCategory = SqLiteService.Get<Category>(x => x.Id == selectedItem.CategoryId);
                    var param = new NavigationParameters
                        {
                            {ParamKey.SelectedItem.ToString(), selectedItem},
                            {ParamKey.Category.ToString(), selectedCategory},
                         //{nameof(StatusOfLeadModel), StatusOfLeadModel.CreateLead},
                        };

                    await Navigation.NavigateAsync(PageManager.ItemDetailsPage, parameters: param);
                }
            });
        }
        #endregion
    }
}
