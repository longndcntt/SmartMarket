using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Extensions;
using SmartMarket.Utilities;
using SmartMarket.Views.Base;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagePopup : PopupBasePage
    {
        public MessagePopup()
        {
            InitializeComponent();
        }

        #region Instance

        private static MessagePopup _instance;

        public static MessagePopup Instance => _instance ?? (_instance = new MessagePopup());

        public async Task<MessagePopup> Show(string message = null, string closeButtonText = null,
            ICommand closeCommand = null, object closeCommandParameter = null,
            bool isAutoClose = false, uint duration = 2000)
        {
            IsClosed = false;

            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                if (message != null)
                    LabelMessage.Text = message;

                if (closeButtonText != null)
                    ButtonClose.Text = closeButtonText;

                ClosedPopupCommand = closeCommand;
                ClosedPopupCommandParameter = closeCommandParameter;

                IsAutoClose = isAutoClose;
                Duration = duration;

                if (isAutoClose && duration > 0)
                    AutoClosePopupAfter(duration);

                // If Loading Popup is showing, then close loading
                if (LoadingPopup.IsLoading)
                    await LoadingPopup.Instance.Hide();

                // Show Message Popup
                await App.Current.MainPage.Navigation.PushPopupAsync(this);
            });

            return this;
        }

        #endregion
    }
}