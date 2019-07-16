using LaavorRatingSwap;
using SmartMarket.ViewModels;
using SmartMarket.Views.Base;
using Xamarin.Forms;

namespace SmartMarket.Views
{
    public partial class ItemDetailsPage : BasePage
    {
        public ItemDetailsPage()
        {
            InitializeComponent();
        }

        private void RatingImage_OnSelect(object sender, System.EventArgs e)
        {
            RatingImage ratingimage = (RatingImage)sender;
            var vm = (ItemDetailsPageViewModel)BindingContext;
            vm.Value = ratingimage.Value;
        }
    }
}
