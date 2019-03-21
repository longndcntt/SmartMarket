using Xamarin.Forms;

namespace SmartMarket.Controls.CustomLabelEntry
{
    public class ExtendedLabel : Label
    {
        #region IsUnderline

        public static readonly BindableProperty IsUnderlineProperty =
            BindableProperty.Create(nameof(IsUnderline), typeof(bool), typeof(ExtendedLabel), false);


        public bool IsUnderline
        {
            get => (bool)GetValue(IsUnderlineProperty);
            set => SetValue(IsUnderlineProperty, value);
        }

        #endregion

        #region Padding

        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ExtendedLabel), new Thickness(10));


        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        #endregion
    }
}
