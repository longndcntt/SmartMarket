using FFImageLoading.Forms;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;

namespace SmartMarket.Views
{
    public partial class ProceedToCheckoutPage : ContentPage
    {
        public ProceedToCheckoutPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = (ProceedToCheckoutPageViewModel)BindingContext;
            var list = vm?.ItemOfOrder;
            LoadItem(list);
        }


        private void LoadItem(ObservableCollection<OrderDetails> list)
        {
            var vm = (ProceedToCheckoutPageViewModel)BindingContext;
            listItem.Children.Clear();
            foreach (var item in list)
            {
                var frame = new Frame()
                {
                    Margin = new Thickness(0, 5)
                };
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                var image = new CachedImage()
                {
                    Source = item.Image,
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 80,
                    WidthRequest = 80,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                grid.Children.Add(image, 0, 0);

                var stack = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Padding = new Thickness(25, 5, 10, 5),
                };

                //Name
                var labelName = new Label()
                {
                    Text = item.Name,
                    FontSize = (double)App.Current.Resources["LargeLabelFont"],
                    TextColor = (Color)App.Current.Resources["SDarkerTextColor"],
                };

                //Manufacter
                var labelManu = new Label()
                {
                    Text = item.Manufacturer,
                    FontSize = (double)App.Current.Resources["NormalLabelFont"],
                    TextColor = (Color)App.Current.Resources["Purple"],
                };

                //Price
                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
                string a = double.Parse(item.Price.ToString()).ToString("#,###", cul.NumberFormat);
                a += "đ";
                var labelPrice = new Label()
                {
                    Text = a,
                    FontSize = (double)App.Current.Resources["LargeLabelFont"],
                    TextColor = (Color)App.Current.Resources["Blue"],
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                };

                //Quantity
                var labelQuantity = new Label()
                {
                    Text = TranslateExtension.Get("Quantity"),
                    FontSize = (double)App.Current.Resources["NormalLabelFont"],
                    TextColor = (Color)App.Current.Resources["SDarkerTextColor"],
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                };
                var labelAmount = new Label()
                {
                    Text = item.Amount.ToString(),
                    FontSize = (double)App.Current.Resources["NormalLabelFont"],
                    TextColor = (Color)App.Current.Resources["SDarkerTextColor"],
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                };
                var stackQuantity = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Horizontal,
                };
                stackQuantity.Children.Add(labelQuantity);
                stackQuantity.Children.Add(labelAmount);

                stack.Children.Add(labelName);
                stack.Children.Add(labelManu);
                stack.Children.Add(labelPrice);
                stack.Children.Add(stackQuantity);
                grid.Children.Add(image, 0, 0);
                grid.Children.Add(stack, 1, 0);
                frame.Content = grid;
                listItem.Children.Add(frame);
            }
        }
    }
}
