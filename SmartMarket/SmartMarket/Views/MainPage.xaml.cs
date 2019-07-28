using DLToolkit.Forms.Controls;
using SmartMarket.Models;
using SmartMarket.ViewModels;
using SmartMarket.ViewModels.Base;
using SmartMarket.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMarket.Views
{
    public partial class MainPage : BasePage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void CarouselViewControl()
        {
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var item = (Frame)sender;
            var vm = (MainPageViewModel)BindingContext;
            vm?.SelectedItemExcute(item.ClassId);
        }

        private void AutoSuggestSearch_SuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            var vm = (MainPageViewModel)BindingContext;
            var itemModel = e.SelectedItem as ItemModel;
            autoSuggestSearch.Text = itemModel.ItemName;
            vm?.SelectedItemExcute(itemModel.Id.ToString());
        }

        private void AutoSuggestSearch_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            var vm = (ViewModelBase)BindingContext;
            vm?.SearchExcute(autoSuggestSearch.Text);
        }

        //private void FlowListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    var vm = (MainPageViewModel)BindingContext;
        //    var item = (ItemModel)e.Item;
        //    if (e.Item == vm?.MyList.Last())
        //    {
        //        vm?.LoadData();
        //    }
        //}
    }
}