using System.Threading.Tasks;
using SmartMarket.Localization;
using SmartMarket.Utilities;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup : PopupPage
    {
        #region Properties

        public static bool IsLoading { get; private set; }

        #endregion

        #region Constructors

        public LoadingPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region Disappear

        protected override void OnDisappearing()
        {
            IsLoading = false;
        }

        #endregion

        #region Instance

        private static LoadingPopup _instance;

        public static LoadingPopup Instance => _instance ?? (_instance = new LoadingPopup());

        public async Task Show(string loadingMessage = null)
        {
            await DeviceExtension.BeginInvokeOnMainThreadAsync(() =>
            {
                LabelMessage.Text = !string.IsNullOrWhiteSpace(loadingMessage)
                    ? loadingMessage : TranslateExtension.Get("Loading");
            });

            if (IsLoading) return;

            IsLoading = true;

            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                Indicator.IsRunning = true;
                await Application.Current.MainPage.Navigation.PushPopupAsync(this);
            });
        }

        #endregion

        #region StopProcessing

        public async Task Hide()
        {
            if (IsLoading)
            {
                await Task.Delay(200);
                IsLoading = false;

                await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                {
                    Indicator.IsRunning = false;
                    await Navigation.PopPopupAsync();
                });
            }
        }

        #endregion

        #region OnBackButtonPressed

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        #endregion

    }
}
