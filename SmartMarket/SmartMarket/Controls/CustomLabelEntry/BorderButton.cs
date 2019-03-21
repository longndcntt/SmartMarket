using SmartMarket.Enums;
using Xamarin.Forms;

namespace SmartMarket.Controls.CustomLabelEntry
{
    public class BorderButton : Button
    {
        #region Theme

        public static readonly BindableProperty ThemeProperty =
            BindableProperty.Create(
                nameof(Theme),
                typeof(BorderButtonTheme),
                typeof(BorderButton),
                BorderButtonTheme.White,
                BindingMode.TwoWay);

        public BorderButtonTheme Theme
        {
            get => (BorderButtonTheme)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        #endregion

        #region Padding

        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(
                nameof(Padding),
                typeof(Thickness),
                typeof(BorderButton),
                new Thickness(15),
                BindingMode.TwoWay);

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        #endregion
    }
}
