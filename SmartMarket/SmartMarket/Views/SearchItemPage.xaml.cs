using SmartMarket.Models;
using SmartMarket.ViewModels;
using SmartMarket.Views.Base;
using Xamarin.Forms;

namespace SmartMarket.Views
{
    public partial class SearchItemPage : BasePage
    {
        public SearchItemPage()
        {
            InitializeComponent();
        }

        private void AutoSuggestSearch_SuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            var vm = (SearchItemPageViewModel)BindingContext;
            var itemModel = e.SelectedItem as ItemModel;
            autoSuggestSearch.Text = itemModel.ItemName;
            vm?.SelectedItemExcute(itemModel.Id.ToString());
        }

        private void AutoSuggestSearch_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            var vm = (SearchItemPageViewModel)BindingContext;
            vm?.SearchExcute(autoSuggestSearch.Text);
        }
    }
}
