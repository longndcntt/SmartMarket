using Prism.Commands;
using Prism.Navigation;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.Utilities;
using SmartMarket.ViewModels.Base;
using SmartMarket.Views.Popups;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SmartMarket.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Constructor
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
            set => SetProperty(ref _selectedItem , value);
        }

        private ObservableCollection<string> _listEvent;
        public ObservableCollection<string> ListEvent
        {
            get => _listEvent;
            set => SetProperty(ref _listEvent, value);
        }
        #endregion
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService: navigationService)
        {
            Title = TranslateExtension.Get("MainPage");
            ItemTappedCommand = new DelegateCommand(ItemTappedExcute);
            MyList = new ObservableCollection<ItemModel>()
            {
                new ItemModel()
                {
                    ItemName = "But Long1",
                    Price = 20000,
                    Image= "006-notification",
                },
                new ItemModel()
                {
                    ItemName = "But Long2",
                    Price = 20000,
                    Image= "006-notification"
                },
                new ItemModel()
                {
                    ItemName = "But Long3",
                    Price = 20000,
                    Image= "006-notification"
                },
                new ItemModel()
                {
                    ItemName = "But Long4",
                    Price = 20000,
                    Image= "006-notification"
                },
                new ItemModel()
                {
                    ItemName = "But Long5",
                    Price = 20000,
                    Image= "006-notification"
                },
                new ItemModel()
                {
                    ItemName = "But Long6",
                    Price = 20000,
                    Image= "006-notification"
                },
            };

            ListEvent = new ObservableCollection<string>()
            {
                "one.png","two.jpg","one.png","two.jpg"
            };

        }

        private void LoginExcute()
        {
            Navigation.NavigateAsync(PageManager.LoginPage);
        }

        public ICommand ItemTappedCommand { get; set; }

        private async void ItemTappedExcute()
        {
            await MessagePopup.Instance.Show(SelectedItemTapped.ItemName);
           // await Navigation.NavigateAsync(PageManager.LoginPage);
        }
    }
}
