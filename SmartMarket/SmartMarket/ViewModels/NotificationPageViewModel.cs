using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartMarket.ViewModels
{
    public class NotificationPageViewModel : ViewModelBase
    {

        public NotificationPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
: base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {
            Time1 = App.Settings.Time1;
            Time2 = App.Settings.Time2;
        }

        #region Properties
        private string _time1;

        public string Time1 { get => _time1; set => SetProperty(ref _time1, value); }

        private string _time2;

        public string Time2 { get => _time2; set => SetProperty(ref _time2, value); }
        #endregion
    }
}
