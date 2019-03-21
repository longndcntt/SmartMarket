using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Extensions;
using SmartMarket.Utilities;
using SmartMarket.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmPopup : PopupBasePage
    {
        private bool _processCloseCommandWhenTouchOutside;

        public ConfirmPopup()
        {
            InitializeComponent();
        }

        
        #region Instance

        private static ConfirmPopup _instance;

        public static ConfirmPopup Instance => _instance ?? (_instance = new ConfirmPopup());

        public async Task<ConfirmPopup> Show(string message = null, string closeButtonText = null,
            ICommand closeCommand = null, object closeCommandParameter = null, bool processCloseCommandWhenTouchOutside = true,
            string acceptButtonText = null, ICommand acceptCommand = null, object acceptCommandParameter = null)
        {
            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                IsClosed = false;
                _processCloseCommandWhenTouchOutside = processCloseCommandWhenTouchOutside;

                if (message != null)
                    LabelMessage.Text = message;

                if (closeButtonText != null)
                    ButtonClose.Text = closeButtonText;

                ClosedPopupCommand = closeCommand;
                ClosedPopupCommandParameter = closeCommandParameter;

                if (acceptButtonText != null)
                    ButtonAccept.Text = acceptButtonText;

                AcceptCommand = acceptCommand;
                AcceptCommandParameter = acceptCommandParameter;

                await App.Current.MainPage.Navigation.PushPopupAsync(this);
            });

            return this;
        }

        #endregion

        #region Events
        private async void AcceptPopupEvent(object sender, EventArgs e)
        {
            IsClosed = true;

            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PopPopupAsync();
            });

            AcceptCommand?.Execute(AcceptCommandParameter);
        }


        private async void TouchOutside(object sender, EventArgs e)
        {
            await ClosedPopup(processCloseCommand: _processCloseCommandWhenTouchOutside);
        }

        #endregion


        #region AcceptCommand

        public static readonly BindableProperty AcceptCommandProperty =
            BindableProperty.Create(nameof(AcceptCommand),
                typeof(ICommand),
                typeof(ConfirmPopup),
                null,
                BindingMode.TwoWay);

        public ICommand AcceptCommand
        {
            get => (ICommand)GetValue(AcceptCommandProperty);
            set => SetValue(AcceptCommandProperty, value);
        }

        public static readonly BindableProperty AcceptCommandParameterProperty =
            BindableProperty.Create(nameof(AcceptCommandParameter),
                typeof(object),
                typeof(ConfirmPopup),
                null,
                BindingMode.TwoWay);

        public object AcceptCommandParameter
        {
            get => GetValue(AcceptCommandParameterProperty);
            set => SetValue(AcceptCommandParameterProperty, value);
        }

        #endregion

    }
}