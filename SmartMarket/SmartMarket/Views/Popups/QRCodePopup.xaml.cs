using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Extensions;
using SVoucher.PCL.Utilities;
using SVoucher.PCL.Views.Base;
using Xamarin.Forms.Xaml;

namespace SVoucher.PCL.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRCodePopup : PopupBasePage
    {
        public QRCodePopup()
        {
            InitializeComponent();
            QRCodeView.BarcodeOptions.Width = 500;
            QRCodeView.BarcodeOptions.Height = 500;
        }
        #region Instance

        private static QRCodePopup _instance;

        public static QRCodePopup Instance => _instance ?? (_instance = new QRCodePopup());

        public async Task<QRCodePopup> Show(string qrCodeValue,
            ICommand closeCommand = null, object closeCommandParameter = null,
            bool isAutoClose = false, uint duration = 2000)
        {
            IsClosed = false;

            if (qrCodeValue == null)
            {
                Debug.WriteLine("[Error][Exception]: QRCodePopup cannot be shown because of null QR code value");
                return null;
            }
            else
            {
                QRCodeView.BarcodeValue = qrCodeValue;
            }

            if (closeCommand != null)
                ClosePopupCommand = closeCommand;

            if (closeCommandParameter != null)
                ClosePopupCommandParameter = closeCommandParameter;

            IsAutoClose = isAutoClose;
            Duration = duration;

            if (isAutoClose && duration > 0)
                AutoClosePopupAfter(duration);

            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(this);
            });

            return this;
        }

        #endregion
    }
}