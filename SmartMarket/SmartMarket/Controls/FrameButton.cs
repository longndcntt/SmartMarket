using System.Threading.Tasks;
using System.Windows.Input;
using SmartMarket.Utilities;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace SmartMarket.Controls
{
    public class FrameButton : Frame
    {
        public FrameButton()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = AnimationCommand
            });
            //Content = new Label()
            //{
            //    VerticalOptions = LayoutOptions.Center,
            //    HorizontalOptions = LayoutOptions.Center,
            //    Text = this.Text,
            //    TextColor = this.TextColor,
            //    FontSize = this.FontSize
            //};
        }

        private ICommand AnimationCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //var opacity = this.Opacity;
                    this.Opacity = 0.4;
                    await Task.Delay(100);
                    this.Opacity = 1;

                    Command?.Execute(CommandParameter);
                    
                });
            }
        }

        #region Command

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
            typeof(ICommand), typeof(FrameButton), null, BindingMode.TwoWay);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region CommandParameter

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FrameButton));
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #region Text

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(FrameButton), string.Empty);
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region TextColor

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FrameButton), Color.Default);
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        #endregion

        #region FontSize

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(FrameButton),(double)13);
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        #endregion

    }
}
