using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartMarket.Controls.InputKit.Abstraction;
using SmartMarket.Controls.InputKit.Configuration;
using SmartMarket.Extensions;
using Xamarin.Forms;

namespace SmartMarket.Controls.InputKit
{
    /// <summary>
    /// A checkbox for boolean inputs. It Includes a text inside
    /// </summary>
    public class CheckBox : StackLayout, IValidatable
    {
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            BackgroundColor = Color.Transparent,
            Color = Color.Accent,
            BorderColor = Color.Black,
            TextColor = Color.Black,
            Size = 25,
            CornerRadius = -1,
            FontSize = (double)App.Current.Resources["NormalLabelFont"],
        };
        
        private Frame boxBackground = new Frame
        {
            Padding = 0, CornerRadius = GlobalSetting.CornerRadius, InputTransparent = true,
            HeightRequest = GlobalSetting.Size, WidthRequest = GlobalSetting.Size,
            BackgroundColor = GlobalSetting.BackgroundColor, MinimumWidthRequest = 35,
            BorderColor = GlobalSetting.BorderColor, VerticalOptions = LayoutOptions.CenterAndExpand, HasShadow = false
        };

        private BoxView boxSelected = new BoxView
        {
            IsVisible = false, HeightRequest = GlobalSetting.Size * .60, WidthRequest = GlobalSetting.Size * .60,
            Color = GlobalSetting.Color, VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.Center
        };

        private Label lblSelected = new Label
        {
            Text = "✓", Margin = new Thickness(0, -1, 0, 0), FontSize = GlobalSetting.Size * 0.8 /*.72*/,
            FontAttributes = FontAttributes.Bold, IsVisible = false, TextColor = GlobalSetting.Color,
            HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.CenterAndExpand,
            FontFamily = Device.RuntimePlatform == Device.iOS ? "ionicons" : "ionicons.ttf#ionicons",
    };

        private Label lblOption = new Label
        {
            VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = GlobalSetting.FontSize,
            TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily, IsVisible = false,
        };
        private CheckType _type = CheckType.Box;
        private bool _isEnabled;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckBox()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.Padding = new Thickness(0, 10);
            this.Spacing = 10;
            boxBackground.Content = boxSelected;
            this.Children.Add(boxBackground);
            //this.Children.Add(new Grid { Children = { boxBackground, boxSelected }, MinimumWidthRequest = 35 });
            this.Children.Add(lblOption);
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (IsDisabled) return;
                    IsChecked = !IsChecked;
                    ExecuteCommand();
                    CheckChanged?.Invoke(this, new EventArgs());
                    ValidationChanged?.Invoke(this, new EventArgs());
                }),
            });
        }

        void ExecuteCommand()
        {
            if (CheckChangedCommand?.CanExecute(CommandParameter ?? this) ?? false)
                CheckChangedCommand?.Execute(CommandParameter ?? this);            
        }

        async void Animate()
        {
            try
            {
                if (Type != CheckType.Material)
                {
                    if (Type != CheckType.Check)
                        return;

                    boxBackground.BackgroundColor = IsChecked ? this.BackgroundColorSelected : this.BoxBackgroundColor;
                    lblOption.TextColor = !IsChecked ? this.CheckedTextColor : this.UnCheckedTextColor;
                    await boxBackground.ScaleTo(1.2, 100, Easing.BounceIn);

                    await Task.Delay(300);
                    boxBackground.Scale = 1.0;
                    return;
                }

                await boxBackground.ScaleTo(0.9, 100, Easing.BounceIn);
                boxBackground.BackgroundColor = IsChecked ? this.Color : Color.Transparent;
                await boxBackground.ScaleTo(1, 100, Easing.BounceIn);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Invoked when check changed
        /// </summary>
        public event EventHandler CheckChanged;
        public event EventHandler ValidationChanged;
       
        /// <summary>
        /// Quick generator constructor
        /// </summary>
        /// <param name="optionName">Text to Display</param>
        /// <param name="key">Value to keep it like an ID</param>
        public CheckBox(string optionName, int key) : this()
        {
            Key = key;
            Text = optionName;
        }

        #region Other properties
       
        /// <summary>
        /// Size of Checkbox
        /// </summary>
        public double BoxSize { get => boxBackground.Width; }
        /// <summary>
        /// SizeRequest of CheckBox
        /// </summary>
        public double BoxSizeRequest { get => boxBackground.WidthRequest; set => SetBoxSize(value); }


        /// <summary>
        /// WARNING! : If you set this as required, user must set checked this control to be validated!
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// Checks if entry required and checked
        /// </summary>
        public bool IsValidated => !this.IsRequired || this.IsChecked;
        /// <summary>
        /// Not available for this control
        /// </summary>
        public string ValidationMessage { get; set; }
        /// <summary>
        /// Fontfamily of CheckBox Text
        /// </summary>
        public string FontFamily { get => lblOption.FontFamily; set => lblOption.FontFamily = value; }

        #endregion

        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        #region Color

        /// <summary>
        /// Color of Checkbox checked
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color),
            typeof(CheckBox), Color.Accent, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateColor());

        void UpdateColor()
        {
            boxSelected.Color = Color;
            if (Type == CheckType.Material)
            {
                boxBackground.BorderColor = Color;
                boxBackground.BackgroundColor = IsChecked ? Color : Color.Transparent;
                lblSelected.TextColor = Color.ToSurfaceColor();
            }
            else
            {
                boxBackground.BorderColor = BorderColor;
                boxBackground.BackgroundColor = BackgroundColor;
                lblSelected.TextColor = Color;
            }
        }

        #endregion

        #region BoxBackgroundColor

        /// <summary>
        /// Checkbox box background color. Default is LightGray
        /// </summary>
        public Color BoxBackgroundColor
        {
            get => (Color)GetValue(BoxBackgroundColorProperty);
            set => SetValue(BoxBackgroundColorProperty, value);
        }

        public static readonly BindableProperty BoxBackgroundColorProperty =
            BindableProperty.Create(nameof(BoxBackgroundColor), typeof(Color), typeof(CheckBox),
                GlobalSetting.BackgroundColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateBoxBackground());

        void UpdateBoxBackground()
        {
            if (this.Type == CheckType.Material)
                return;
            boxBackground.BackgroundColor = BoxBackgroundColor;
        }

        #endregion

        #region BackgroundColorSelected

        /// <summary>
        /// Checkbox box background color. Default is LightGray
        /// </summary>
        public Color BackgroundColorSelected
        {
            get => (Color)GetValue(BackgroundColorSelectedProperty);
            set => SetValue(BackgroundColorSelectedProperty, value);
        }

        public static readonly BindableProperty BackgroundColorSelectedProperty =
            BindableProperty.Create(nameof(BackgroundColorSelected), typeof(Color), typeof(CheckBox),
                GlobalSetting.BackgroundColor);

        #endregion

        #region BorderColor

        /// <summary>
        /// Border color of around CheckBox
        /// </summary>
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
            typeof(Color), typeof(CheckBox), GlobalSetting.BorderColor,
            propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateBorderColor());

        void UpdateBorderColor()
        {
            if (this.Type == CheckType.Material) return;
            boxBackground.BorderColor = this.BorderColor;
        }

        #endregion

        #region UnCheckedTextColor

        /// <summary>
        /// Color of text
        /// </summary>
        public Color UnCheckedTextColor
        {
            get => (Color)GetValue(UnCheckedTextColorProperty);
            set => SetValue(UnCheckedTextColorProperty, value);
        }

        public static readonly BindableProperty UnCheckedTextColorProperty = BindableProperty.Create(nameof(UnCheckedTextColor),
            typeof(Color), typeof(CheckBox), GlobalSetting.TextColor,
            propertyChanged: (bo, ov, nv) => ((CheckBox) bo).UnCheckedTextColor = (Color) nv);

        #endregion

        #region CheckedTextColor

        /// <summary>
        /// Color of text
        /// </summary>
        public Color CheckedTextColor
        {
            get => (Color)GetValue(CheckedTextColorProperty);
            set => SetValue(CheckedTextColorProperty, value);
        }

        public static readonly BindableProperty CheckedTextColorProperty = BindableProperty.Create(nameof(CheckedTextColor),
            typeof(Color), typeof(CheckBox), GlobalSetting.TextColor,
            propertyChanged: (bo, ov, nv) => ((CheckBox)bo).CheckedTextColor = (Color)nv);

        #endregion

        #region TextFontSize

        /// <summary>
        /// Fontsize of Checkbox text
        /// </summary>
        public double TextFontSize
        {
            get => lblOption.FontSize;
            set => lblOption.FontSize = value;
        }

        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize),
            typeof(double), typeof(CheckBox), 14.0,
            propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextFontSize = (double)nv);

        #endregion

        #region IsChecked

        /// <summary>
        /// IsChecked Property
        /// </summary>
        public bool IsChecked
        {
            get => boxBackground.Content.IsVisible;
            set
            {
                boxBackground.Content.IsVisible = value;
                SetValue(IsCheckedProperty, value);
                Animate();
            }
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked),
            typeof(bool), typeof(CheckBox), false, BindingMode.TwoWay,
            propertyChanged: (bo, ov, nv) => (bo as CheckBox).IsChecked = (bool) nv);

        #endregion

        #region IsDisabled

        /// <summary>
        /// Gets or sets the checkbutton enabled or not. If checkbox is disabled, checkbox can not be interacted.
        /// </summary>
        public bool IsDisabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                this.Opacity = value ? 0.6 : 1;
            }
        }

        public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled),
            typeof(bool), typeof(CheckBox), false,
            propertyChanged: (bo, ov, nv) => (bo as CheckBox).IsDisabled = (bool)nv);

        #endregion

        #region Key

        /// <summary>
        /// You can set a Unique key for each control
        /// </summary>
        public int Key { get; set; }

        public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(int),
            typeof(CheckBox), 0, propertyChanged: (bo, ov, nv) => (bo as CheckBox).Key = (int) nv);

        #endregion

        #region Text

        /// <summary>
        /// Text to display right of CheckBox
        /// </summary>
        public string Text
        {
            get => lblOption.Text;
            set
            {
                lblOption.Text = value;
                lblOption.IsVisible = !String.IsNullOrEmpty(value);
            }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string),
            typeof(CheckBox), "", propertyChanged: (bo, ov, nv) => (bo as CheckBox).Text = (string) nv);

        #endregion

        #region CheckChangedCommand

        /// <summary>
        /// Executed when check changed
        /// </summary>
        public ICommand CheckChangedCommand { get; set; }

        public static readonly BindableProperty CheckChangedCommandProperty =
            BindableProperty.Create(nameof(CheckChangedCommand), typeof(ICommand), typeof(CheckBox), null,
                propertyChanged: (bo, ov, nv) => (bo as CheckBox).CheckChangedCommand = (ICommand) nv);

        #endregion

        #region CommandParameter

        /// <summary>
        /// Command Parameter for Commands. If this is null, CommandParameter will be sent as itself of CheckBox
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CheckBox), null);

        #endregion

        #region Type

        /// <summary>
        /// Which icon will be shown when checkbox is checked
        /// </summary>
        public CheckType Type
        {
            get => _type;
            set
            {
                _type = value;
                UpdateType(value);
            }
        }

        void UpdateType(CheckType _Type)
        {
            switch (_Type)
            {
                case CheckType.Box:
                    boxBackground.Content = boxSelected;
                    break;
                case CheckType.Check:
                    lblSelected.Text = "✓";
                    boxBackground.Content = lblSelected;
                    break;
                case CheckType.Cross:
                    lblSelected.Text = "✕";
                    boxBackground.Content = lblSelected;
                    break;
                case CheckType.Star:
                    lblSelected.Text = "★";
                    boxBackground.Content = lblSelected;
                    break;
                case CheckType.Material:
                    lblSelected.Text = "✓";
                    boxBackground.CornerRadius = 5;
                    boxBackground.Content = lblSelected;
                    break;
            }
            UpdateAllColors();
        }

        #endregion

        #region UpdateAllColor

        void UpdateAllColors()
        {
            UpdateColor();
            UpdateBoxBackground();
            UpdateBorderColor();
        }

        #endregion

        #region SetBoxSize
        void SetBoxSize(double value)
        {
            boxBackground.WidthRequest = value;
            boxBackground.HeightRequest = value;
            boxSelected.WidthRequest = value * .6;  //old value 0.72
            boxSelected.HeightRequest = value * 0.6;
            lblSelected.FontSize = value * 0.72;       //old value 0.76
            this.Children[0].MinimumWidthRequest = value * 1.4;
        }

        #endregion

        /// <summary>
        /// Not available for this control
        /// </summary>
        public void DisplayValidation()
        {

        }

        public enum CheckType
        {
            Box,
            Check,
            Cross,
            Star,
            Material
        }
    }
}
