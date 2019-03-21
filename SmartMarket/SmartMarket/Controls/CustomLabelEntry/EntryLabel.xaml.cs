using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Controls.CustomLabelEntry
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntryLabel : ContentView
	{
		public EntryLabel()
		{
			InitializeComponent();
		}
        #region Text

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryLabel), string.Empty,
                BindingMode.TwoWay, propertyChanged: TextChanged);

        private static void TextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (EntryLabel)bindable;
            control.Text = (string)newvalue;
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                EntryEntry.Text = value;
            }
        }

        private void EntryEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = e.NewTextValue;
        }

        #endregion

        #region TextColor

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(EntryLabel), Color.FromHex("#5b5b5b"),
                propertyChanged: TextColorChanged);

        private static void TextColorChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (EntryLabel)bindable;
            control.TextColor = (Color)newvalue;
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set
            {
                SetValue(TextColorProperty, value);
                EntryEntry.TextColor = value;
            }
        }

        #endregion

        #region Placeholder

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EntryLabel), string.Empty,
                propertyChanged: PlaceholderChanged);

        private static void PlaceholderChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (EntryLabel)bindable;
            control.Placeholder = (string)newvalue;
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set
            {
                SetValue(PlaceholderProperty, value);
                EntryEntry.Placeholder = value;
            }
        }

        #endregion

        #region PlaceholderColor

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(EntryLabel), Color.FromHex("#B5B5B5"),
                propertyChanged: PlaceholderColorChanged);

        private static void PlaceholderColorChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (EntryLabel)bindable;
            control.PlaceholderColor = (Color)newvalue;
        }

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set
            {
                SetValue(PlaceholderColorProperty, value);
                EntryEntry.PlaceholderColor = value;
            }
        }

        #endregion

        #region LabelText

        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(EntryLabel), string.Empty,
                propertyChanged: LabelTextChanged);

        private static void LabelTextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (EntryLabel)bindable;
            control.LabelText = (string)newvalue;
        }

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set
            {
                SetValue(LabelTextProperty, value);
                LabelLabel.Text = value;
            }
        }

        #endregion

        #region LabelTextColor

        public static readonly BindableProperty LabelTextColorProperty =
            BindableProperty.Create(nameof(LabelTextColor), typeof(Color), typeof(EntryLabel), Color.FromHex("#B5B5B5"),
                propertyChanged: LabelTextColorChanged);

        private static void LabelTextColorChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (EntryLabel)bindable;
            control.LabelTextColor = (Color)newvalue;
        }

        public Color LabelTextColor
        {
            get => (Color)GetValue(LabelTextColorProperty);
            set
            {
                SetValue(LabelTextColorProperty, value);
                LabelLabel.TextColor = value;
            }
        }

        #endregion


        #region LabelTextColor

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(EntryLabel),
                propertyChanged: IconChanged);

        private static void IconChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (EntryLabel)bindable;
            control.Icon = (ImageSource)newvalue;
        }

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set
            {
                SetValue(IconProperty, value);
                ImageIcon.Source = value;
            }
        }

        #endregion

        #region IsTopLineVisible

        public static readonly BindableProperty IsTopLineVisibleProperty =
            BindableProperty.Create(nameof(IsTopLineVisible), typeof(bool), typeof(EntryLabel), false,
                propertyChanged: IsTopLineVisibleChanged);

        private static void IsTopLineVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (EntryLabel)bindable;
            control.IsTopLineVisible = (bool)newvalue;
        }

        public bool IsTopLineVisible
        {
            get => (bool)GetValue(IsTopLineVisibleProperty);
            set
            {
                SetValue(IsTopLineVisibleProperty, value);
                TopLine.IsVisible = value;
            }
        }

        #endregion

        #region IsBottomLineVisible

        public static readonly BindableProperty IsBottomLineVisibleProperty =
            BindableProperty.Create(nameof(IsBottomLineVisible), typeof(bool), typeof(EntryLabel), false,
                propertyChanged: IsBottomLineVisibleChanged);

        private static void IsBottomLineVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EntryLabel)bindable;
            control.IsBottomLineVisible = (bool)newValue;
        }

        public bool IsBottomLineVisible
        {
            get => (bool)GetValue(IsBottomLineVisibleProperty);
            set
            {
                SetValue(IsBottomLineVisibleProperty, value);
                BottomLine.IsVisible = value;
            }
        }

        #endregion
    }
}