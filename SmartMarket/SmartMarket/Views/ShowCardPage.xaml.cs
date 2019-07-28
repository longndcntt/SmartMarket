using FFImageLoading.Forms;
using SmartMarket.Controls.CustomLabelEntry;
using SmartMarket.Localization;
using SmartMarket.Models;
using SmartMarket.ViewModels;
using SmartMarket.Views.Base;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;

namespace SmartMarket.Views
{
    public partial class ShowCardPage : BasePage
    {
        public ShowCardPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = (ShowCardPageViewModel)BindingContext;
            var list = vm?.ItemOfOrder;
            LoadItem(list);
        }


        private void LoadItem(ObservableCollection<OrderDetails> list)
        {
            var vm = (ShowCardPageViewModel)BindingContext;
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
                var stackPrice = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Horizontal,
                };
                var labelPrice = new Label()
                {
                    Text = item.Price.ToString(),
                    FontSize = (double)App.Current.Resources["LargeLabelFont"],
                    TextColor = (Color)App.Current.Resources["Blue"],
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                };
                var imagePrice = new Image()
                {
                    Source = "ic_coin",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HeightRequest = 25,
                    WidthRequest = 25,
                };
                stackPrice.Children.Add(labelPrice);
                stackPrice.Children.Add(imagePrice);
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

                var stackControl = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 0,
                };
                var button = new BorderButton()
                {
                    Text = TranslateExtension.Get("Delete"),
                    Theme = Enums.BorderButtonTheme.RedWhite,
                    TextColor = Color.FromHex("#D50000"),
                    FontSize = 16,
                    Padding = new Thickness(0, 0, 0, 0),
                    CornerRadius = 0,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    ClassId = item.Id,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                button.Clicked += (s, e) =>
                {
                    foreach (var item1 in list)
                    {
                        if (item1.Id == button.ClassId)
                        {
                            vm?.DeleteItemOfOrder(item1.Id);
                            OnAppearing();
                            break;
                        }
                    }
                };
                var labelItems = new Label()
                {
                    Text = TranslateExtension.Get("Items"),
                    FontSize = (double)App.Current.Resources["NormalLabelFont"],
                    TextColor = (Color)App.Current.Resources["SDarkerTextColor"],
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(0, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                };
                var entryEditQuantity = new Entry()
                {
                    Text = item.Amount.ToString(),
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    FontSize = (double)App.Current.Resources["NormalLabelFont"],
                    Keyboard = Keyboard.Numeric,
                    Margin = new Thickness(0, 0, 0, 0)
                };
                entryEditQuantity.TextChanged += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(entryEditQuantity.Text))
                    {
                        foreach (var item1 in list)
                        {
                            if (item1.Id == button.ClassId)
                            {
                                var newAmount = Int32.Parse(entryEditQuantity.Text);
                                vm?.EditQuantityItemOfOrder(item1.Id, newAmount);
                                labelAmount.Text = entryEditQuantity.Text;
                                break;
                            }
                        }
                    }
                };
                stackControl.Children.Add(entryEditQuantity);
                stackControl.Children.Add(labelItems);
                stackControl.Children.Add(button);

                stack.Children.Add(labelName);
                stack.Children.Add(labelManu);
                stack.Children.Add(stackPrice);
                stack.Children.Add(stackQuantity);
                stack.Children.Add(stackControl);
                grid.Children.Add(image, 0, 0);
                grid.Children.Add(stack, 1, 0);
                frame.Content = grid;
                listItem.Children.Add(frame);
            }
        }
    }
}
