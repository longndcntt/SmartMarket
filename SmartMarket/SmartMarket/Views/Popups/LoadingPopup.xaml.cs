using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SmartMarket.Localization;
using SmartMarket.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup : PopupPage
    {
        public static bool IsLoading { get; private set; }

        public LoadingPopup()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            IsLoading = false;
        }

        #region Instance

        private static LoadingPopup _instance;

        public static LoadingPopup Instance => _instance ?? (_instance = new LoadingPopup());

        public async Task Show(string loadingMessage = null)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                LabelLoadingMessage.Text = !string.IsNullOrWhiteSpace(loadingMessage)
                    ? loadingMessage : TranslateExtension.Get("Loading3dot");
            });

            if (IsLoading) return;

            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                LoadingIndicator.IsRunning = true;
                IsLoading = true;

                await App.Current.MainPage.Navigation.PushPopupAsync(this);
            });
        }

        #endregion

        #region StopProcessing

        public async Task Hide()
        {
            if(IsLoading)
            {
                await Task.Delay(200);

                await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                {
                    LoadingIndicator.IsRunning = false;
                    IsLoading = false;
                    await Navigation.PopPopupAsync();
                });
            }
        }

        #endregion

        // Lock hard ware back button
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}