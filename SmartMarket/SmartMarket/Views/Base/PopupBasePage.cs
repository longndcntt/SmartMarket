using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartMarket.Utilities;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace SmartMarket.Views.Base
{
    public class PopupBasePage : PopupPage
    {
        #region Constructors

        public PopupBasePage()
        { }

        #endregion

        #region Properties

        protected bool IsAutoClose;
        protected bool IsClosed;
        protected uint Duration;
        protected int _popupId;

        #endregion

        #region Events

        protected async void ClosedPopupEvent(object sender, EventArgs e)
        {
            await ClosedPopup();
        }

        protected void DoNothingEvent(object sender, EventArgs e)
        {
            // Do nothing
        }

        protected async void AutoClosePopupAfter(uint duration)
        {
            int id = _popupId;
            await Task.Delay((int)duration);

            // If the popup still appear after duration time then close the popup
            if (_popupId == id)
                await ClosedPopup();
        }

        #endregion

        #region ClosedPopupCommand

        public static readonly BindableProperty ClosedPopupCommandProperty =
            BindableProperty.Create(nameof(ClosedPopupCommand),
                typeof(ICommand),
                typeof(PopupBasePage),
                null,
                BindingMode.TwoWay);

        public ICommand ClosedPopupCommand
        {
            get => (ICommand)GetValue(ClosedPopupCommandProperty);
            set => SetValue(ClosedPopupCommandProperty, value);
        }

        public static readonly BindableProperty ClosedPopupCommandParameterProperty =
            BindableProperty.Create(nameof(ClosedPopupCommandParameter),
                typeof(object),
                typeof(PopupBasePage),
                null,
                BindingMode.TwoWay);

        public object ClosedPopupCommandParameter
        {
            get => GetValue(ClosedPopupCommandParameterProperty);
            set => SetValue(ClosedPopupCommandParameterProperty, value);
        }

        protected async Task ClosedPopup(bool processCloseCommand = true)
        {
            IsClosed = true;

            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PopPopupAsync();
            });

            if (processCloseCommand)
                ClosedPopupCommand?.Execute(ClosedPopupCommandParameter);

            _popupId++;
        }

        #endregion
    }
}
