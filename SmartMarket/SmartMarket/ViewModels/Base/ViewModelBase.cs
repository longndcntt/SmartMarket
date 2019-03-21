using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using FotoScan.Tablet.Interfaces;
using FotoScan.Tablet.Interfaces.LocalDatabase;
using Plugin.SecureStorage;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmartMarket.Interfaces.HttpService;
using SmartMarket.Models;
using SmartMarket.Utilities;

namespace SmartMarket.ViewModels.Base
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, INotifyPropertyChanged
    {
        #region Properties Services

        //protected INavigationService NavigationService { get; private set; }
        public INavigationService Navigation { get; private set; }
        public IPageDialogService DialogService { get; private set; }
        public IHttpRequest HttpRequest { get; private set; }
        public ISqLiteService SqLiteService { get; private set; }
        public IAppInfo AppInfo { get; private set; }

        #endregion

        #region Properties
        private string _webUrl;
        public string WebUrl
        {
            get => _webUrl;
            set => SetProperty(ref _webUrl, value);
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private UserModel _userInfo;
        public UserModel UserInfo
        {
            get => _userInfo;
            set => SetProperty(ref _userInfo, value);
        }


        #endregion

        #region Constructor

        public ViewModelBase(INavigationService navigationService = null, IPageDialogService dialogService = null,
            IHttpRequest httpRequest = null, ISqLiteService sqliteService = null, IAppInfo appInfo = null)
        {
            //PageTitle = TranslateExtension.Get("FairflexxFotoScan");
            if (navigationService != null) Navigation = navigationService;
            if (dialogService != null) DialogService = dialogService;
            if (httpRequest != null) HttpRequest = httpRequest;
            if (sqliteService != null) SqLiteService = sqliteService;
            if (appInfo != null) AppInfo = appInfo;

            OpenSettingsPageCommand = new DelegateCommand(async () => await OpenSettingsPageExe());
            BackCommand = new DelegateCommand(async () => await BackExecute());
            ZoomImageCommand = new DelegateCommand<object>(ZoomImageExe);
        }

        #endregion

        #region CheckBusy

        protected async Task CheckBusy(Func<Task> function)
        {
            if (App.IsBusy)
            {
                App.IsBusy = false;
                try
                {
                    await function();
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine(e);
#endif
                }
                finally
                {
                    App.IsBusy = true;
                }
            }
        }

        #endregion


        #region Check Permission

        protected async void CheckPermission(Action action)
        {
            //await CheckBusy(async () =>
            //{
            //    var camera = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            //    var storage = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            //    if (storage == PermissionStatus.Granted)
            //    {
            //        action();
            //    }
            //    else
            //    {
            //        await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
            //    }
            //});
        }

        #endregion

        #region Navigate

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigated from");
#endif
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigating to");
#endif
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigated to");
#endif 

            if (parameters != null)
            {
                var navMode = parameters.GetNavigationMode();
                switch (navMode)
                {
                    case NavigationMode.New: OnNavigatedNewTo(parameters); break;
                    case NavigationMode.Back: OnNavigatedBackTo(parameters); break;
                }
            }

        }

        public virtual void OnNavigatedNewTo(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigate new to");
#endif
        }

        public virtual void OnNavigatedBackTo(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigate back to");
#endif
        }

        #endregion

        #region OnAppear / Disappear

        public virtual void OnAppear()
        {

        }

        public virtual void OnFirstTimeAppear()
        {

        }

        public virtual void OnDisappear()
        {

        }

        #endregion

        #region BackCommand

        public ICommand BackCommand { get; }

        protected virtual async Task BackExecute()
        {
            await CheckBusy(async () =>
            {
                await Navigation.GoBackAsync();
            });
        }

        #endregion

        #region BackButtonPress

        /// <summary>
        /// //false is default value when system call back press
        /// </summary>
        /// <returns></returns>
        public virtual bool OnBackButtonPressed()
        {
            //false is default value when system call back press
            //return false;
            //BackExecute();
            Task.Run(async () =>
            {
                await BackExecute();
            });

            return true;

        }

        /// <summary>
        /// called when page need override soft back button
        /// </summary>
        public virtual void OnSoftBackButtonPressed() { }

        #endregion

        #region Zoom

        public ICommand ZoomImageCommand { get; }
        protected virtual void ZoomImageExe(object obj)
        {

        }

        #endregion

        #region SettingsCommand

        //public ICommand SettingsCommand { get; set; }

        //public virtual async void SettingsExe()
        //{
        //    await CheckBusy(async () => await Navigation.NavigateAsync(PageManager.SettingsPage));
        //}

        #endregion

        #region AddHeaderToken

        public void AddHeaderToken(string token)
        {
            var checkHeader = HttpRequest.DefaultRequestHeaders.Authorization;

            if (checkHeader != null)
            {
                HttpRequest.DefaultRequestHeaders.Remove("Authorization");
            }

            //var tokenExpired =
            //    "bearer SmAvxJyV4O3mSxnc1ScUFSQP6xYJvdBLgf6nsB-b7AJDz_xqh_4PDozu6lr8OkFQ_rrQTqp31GsPiUNl-kxyIaVQFlmwZqWbq1xzpzCiqkavN6AwNoxI2bk983XLo4l3tERounXWl6-haJhbIknicJsafLTqzopAd0WMnEVodb94XpOh6Ss53AQ02GFjkKwNRcM8AQlrvdBLxiM774_fi1wRbNEf0Fcp_CIsOs0AlmkfkLMd-bpvwitDAVwXoFpOIJ5XnwULsoe4cHbVUgwsA";
            CrossSecureStorage.Current.SetValue("AccessToken", token);
            HttpRequest.DefaultRequestHeaders.Add("Authorization", token);
        }

        #endregion

        #region Token Expired

        public bool IsTokenExpire { get; set; } = false;

        #region Properties

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        public async Task LogOutAsync(string pageViewModel = null)
        {
            await Task.Run(async () =>
            {
                // todo: Call API Logout
                SqLiteService.DeleteAll<UserModel>();
                //SqLiteService.DeleteAll<EventModel>();

                ////todo: Delete token header
                HttpRequest.DefaultRequestHeaders.Remove("Authorization");
                CrossSecureStorage.Current.SetValue("AccessToken", string.Empty);

                UserInfo = null;

                Username = Password = string.Empty;
                App.Settings.IsLogin = false;
                SqLiteService.Update(App.Settings);

                // Navigate to Login page
                //if (!(pageViewModel.Contains(PageManager.SettingsPage)))
                //{
                //    await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                //    {
                //        await OpenSettingsPageExe();
                //    });
                //}

            });
        }

        #region OpenSettingsPageCommand

        public ICommand OpenSettingsPageCommand { get; set; }

        public virtual async Task OpenSettingsPageExe()
        {
            await Navigation.NavigateAsync(PageManager.TabbedPage);
        }

        #endregion

        #endregion

        #region Destroy

        public virtual void Destroy()
        {

        }

        #endregion
    }
}
