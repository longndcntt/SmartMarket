using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Views.Base
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HeaderView : ContentView
	{
		public HeaderView()
		{
			InitializeComponent();
        }

	    #region HeaderTitle

	    /// <summary>
	    /// Create PageHeaderTitleProperty to set title for page
	    /// </summary>
	    public string PageHeaderTitle
	    {
	        get => (string)GetValue(PageHeaderTitleProperty);
	        set => SetValue(PageHeaderTitleProperty, value);
	    }

	    public static readonly BindableProperty PageHeaderTitleProperty =
	        BindableProperty.Create(nameof(PageHeaderTitle),
	            typeof(string),
	            typeof(HeaderView),
	            string.Empty,
	            BindingMode.TwoWay,
	            propertyChanged: OnPageHeaderTitleChanged);

	    private static void OnPageHeaderTitleChanged(BindableObject bindable, object oldValue, object newValue)
	    {
	        ((HeaderView)bindable).headerTitle.Text = newValue.ToString();
	    }

        #endregion

        #region BackImageSource

        /// <summary>
        /// 
        /// </summary>
        public string BackImageSource
        {
	        get => GetValue(BackImageProperty).ToString();
	        set => SetValue(BackImageProperty, value);
	    }

	    public static readonly BindableProperty BackImageProperty =
	        BindableProperty.Create(nameof(BackImageSource),
	            typeof(string),
	            typeof(HeaderView),
	            "",
	            BindingMode.TwoWay,
                propertyChanged: OnBackImageSourcePropertyChanged);

	    private static void OnBackImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	    {
	        ((HeaderView) bindable).backImage.Source = ImageSource.FromFile(newValue.ToString());
	    }

        #endregion

        #region IsBackImageVisible

        /// <summary>
        /// 
        /// </summary>
        public bool IsBackImageVisible
        {
	        get => (bool)GetValue(IsBackImageVisibleProperty);
	        set => SetValue(IsBackImageVisibleProperty, value);
	    }

	    public static readonly BindableProperty IsBackImageVisibleProperty =
	        BindableProperty.Create(nameof(IsBackImageVisible),
	            typeof(bool),
	            typeof(HeaderView),
	            true,
                BindingMode.TwoWay,
	            propertyChanged: OnIsBackImageVisiblePropertyChanged);

	    private static void OnIsBackImageVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	    {
	        ((HeaderView)bindable).backImage.IsVisible = (bool)newValue;
	    }
        #endregion

        #region BackImageCommand

        /// <summary>
        /// 
        /// </summary>
        public ICommand BackImageCommand
        {
	        get => (ICommand)GetValue(BackImageCommandProperty);
	        set => SetValue(BackImageCommandProperty, value);
	    }

	    public static readonly BindableProperty BackImageCommandProperty =
	        BindableProperty.Create(nameof(BackImageCommand),
	            typeof(ICommand),
	            typeof(HeaderView),
	            propertyChanged: OnBackImageCommandPropertyChanged);

	    private static void OnBackImageCommandPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
	    {
            ((HeaderView)bindable).backBox.GestureRecognizers.Add
	            (new TapGestureRecognizer() { Command = (ICommand)newvalue,
                    NumberOfTapsRequired = 1
                });
        }
        #endregion
        
        #region NextImageSource

        /// <summary>
        /// 
        /// </summary>
        public string NextImageSource
        {
            get => GetValue(NextImageProperty).ToString();
            set => SetValue(NextImageProperty, value);
        }

        public static readonly BindableProperty NextImageProperty =
            BindableProperty.Create(nameof(NextImageSource),
                typeof(string),
                typeof(HeaderView),
                "",
                BindingMode.TwoWay,
                propertyChanged: OnNextImageSourcePropertyChanged);

        private static void OnNextImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((HeaderView)bindable).nextImage.Source = ImageSource.FromFile(newValue.ToString());
        }

        #endregion

        #region IsNextImageVisible

        /// <summary>
        /// 
        /// </summary>
        public bool IsNextImageVisible
        {
            get => (bool)GetValue(IsNextImageVisibleProperty);
            set => SetValue(IsNextImageVisibleProperty, value);
        }

        public static readonly BindableProperty IsNextImageVisibleProperty =
            BindableProperty.Create(nameof(IsNextImageVisible),
                typeof(bool),
                typeof(HeaderView),
                false,
                BindingMode.TwoWay,
                propertyChanged: OnIsNextImageSourceVisiblePropertyChanged);

        private static void OnIsNextImageSourceVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((HeaderView)bindable).nextImage.IsVisible = (bool)newValue;

        }
        #endregion

        #region NextImageCommand

        /// <summary>
        /// 
        /// </summary>
        public ICommand NextImageCommand
        {
            get => (ICommand)GetValue(NextImageCommandProperty);
            set => SetValue(NextImageCommandProperty, value);
        }

        public static readonly BindableProperty NextImageCommandProperty =
            BindableProperty.Create(nameof(NextImageCommand),
                typeof(ICommand),
                typeof(HeaderView),
                propertyChanged: OnNextImageCommandPropertyChanged);
        private static void OnNextImageCommandPropertyChanged(BindableObject bindable, object oldValue, object newvalue)
        {
            ((HeaderView)bindable).nextBox.GestureRecognizers.Add
                (new TapGestureRecognizer()
                {
                    Command = (ICommand)newvalue,
                    NumberOfTapsRequired = 1
                });
        }
        #endregion
    }
}