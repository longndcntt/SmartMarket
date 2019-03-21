using SmartMarket.Enums;
using Xamarin.Forms;

namespace SmartMarket.Controls.CustomLabelEntry
{
    public class BorderEditor : Editor
    {
        #region Icon

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(string), typeof(BorderEditor), string.Empty);


        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        #endregion

        #region IconHeight

        public int IconHeight
        {
            get => (int)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        public static readonly BindableProperty IconHeightProperty =
            BindableProperty.Create(nameof(IconHeight), typeof(int), typeof(BorderEditor), 40);

        #endregion

        #region IconWidth

        public int IconWidth
        {
            get => (int)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        public static readonly BindableProperty IconWidthProperty =
            BindableProperty.Create(nameof(IconWidth), typeof(int), typeof(BorderEditor), 40);

        #endregion


        #region Padding

        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(
                nameof(Padding),
                typeof(Thickness),
                typeof(BorderEditor),
                new Thickness(5, 10, 5, 10),
                BindingMode.TwoWay);

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        #endregion


        #region MaxLength

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }


        public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(BorderEditor), 100);

        #endregion

        #region IconSpacing

        public static readonly BindableProperty IconSpacingProperty =
            BindableProperty.Create(
                nameof(IconSpacing),
                typeof(int),
                typeof(BorderEditor),
                20,
                BindingMode.TwoWay);

        public int IconSpacing
        {
            get => (int)GetValue(IconSpacingProperty);
            set => SetValue(IconSpacingProperty, value);
        }

        #endregion

        #region Theme

        public static readonly BindableProperty ThemeProperty =
            BindableProperty.Create(
                nameof(Theme),
                typeof(BorderEntryTheme),
                typeof(BorderEditor),
                BorderEntryTheme.WhiteTransparent,
                BindingMode.TwoWay);

        public BorderEntryTheme Theme
        {
            get => (BorderEntryTheme)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        #endregion
    }
}
