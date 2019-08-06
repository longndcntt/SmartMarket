using SmartMarket.Controls.CustomLabelEntry;
using SmartMarket.ViewModels;
using SmartMarket.Views.Base;
using Xamarin.Forms;

namespace SmartMarket.Views
{
    public partial class GoToShopPage : BasePage
    {
        public GoToShopPage()
        {
            InitializeComponent();
        }

        private void DeleteProduct_Clicked(object sender, System.EventArgs e)
        {
            var id = (sender as Button).ClassId;
            var vm = (GoToShopPageViewModel)BindingContext;
            vm?.DeleteItemExcute(id);
        }
    }
}
