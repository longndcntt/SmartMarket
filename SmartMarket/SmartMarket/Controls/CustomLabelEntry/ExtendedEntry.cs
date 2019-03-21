using Xamarin.Forms;

namespace SmartMarket.Controls.CustomLabelEntry
{
    public class ExtendedEntry : Entry
    {

        #region Borderless

        public static readonly BindableProperty BorderlessProperty =
            BindableProperty.Create(nameof(Borderless), typeof(bool), typeof(ExtendedEntry), false);


        public bool Borderless
        {
            get => (bool)GetValue(BorderlessProperty);
            set => SetValue(BorderlessProperty, value);
        }

        #endregion
        
        #region Padding

        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ExtendedEntry), new Thickness(10));


        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        #endregion
    }
}
