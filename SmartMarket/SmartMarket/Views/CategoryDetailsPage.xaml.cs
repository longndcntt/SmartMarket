using SmartMarket.ViewModels;
using SmartMarket.Views.Base;
using Xamarin.Forms;

namespace SmartMarket.Views
{
    public partial class CategoryDetailsPage : BasePage
    {
        public CategoryDetailsPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var item = (Frame)sender;
            var vm = (CategoryDetailsPageViewModel)BindingContext;
            vm?.SelectedItemExcute(item.ClassId);
        }
    }
}
