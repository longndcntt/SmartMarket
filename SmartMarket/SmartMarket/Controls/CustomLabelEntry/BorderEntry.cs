using SmartMarket.Enums;
using Xamarin.Forms;

namespace SmartMarket.Controls.CustomLabelEntry
{
    public class BorderEntry : Entry
    {
        public BorderEntry()
        { }

        #region Icon

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(string), typeof(BorderEntry), string.Empty);


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
            BindableProperty.Create(nameof(IconHeight), typeof(int), typeof(BorderEntry), 40);

        #endregion

        #region IconWidth

        public int IconWidth
        {
            get => (int)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        public static readonly BindableProperty IconWidthProperty =
            BindableProperty.Create(nameof(IconWidth), typeof(int), typeof(BorderEntry), 40);

        #endregion


        #region Padding

        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(
                nameof(Padding),
                typeof(Thickness),
                typeof(BorderEntry),
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
            BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(BorderEntry), 100);

        #endregion

        #region IconSpacing

        public static readonly BindableProperty IconSpacingProperty =
            BindableProperty.Create(
                nameof(IconSpacing),
                typeof(int),
                typeof(BorderEntry),
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
                typeof(BorderEntry),
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
