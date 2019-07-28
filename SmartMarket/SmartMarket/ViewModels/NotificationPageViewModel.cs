using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Interfaces.LocalDatabase;
using SmartMarket.Models;
using SmartMarket.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartMarket.ViewModels
{
    public class NotificationPageViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<NotificationModel> _notificationList;
        public ObservableCollection<NotificationModel> NotificationList
        {
            get => _notificationList;
            set => SetProperty(ref _notificationList, value);
        }
        #endregion
        public NotificationPageViewModel(INavigationService navigationService, ISqLiteService sqLiteService, IHttpRequest httpRequest)
: base(navigationService: navigationService, sqliteService: sqLiteService, httpRequest: httpRequest)
        {

        }

        public override void OnAppear()
        {
            base.OnAppear();
            var a = SqLiteService.GetList<NotificationModel>(x => x.Message != null || x.Message != "");
            if (a != null)
            {
                NotificationList = new ObservableCollection<NotificationModel>(a.OrderByDescending(x => x.DateTimeSend));
            }
        }
    }
}
