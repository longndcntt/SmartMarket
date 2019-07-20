using System.IO;
using System.Threading.Tasks;
using SmartMarket.Utilities;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZoomImagePopUp : PopupPage
    {
        public ZoomImagePopUp(ImageSource imgSource)
        {
            InitializeComponent();
            ViewImagePopUp.Source = imgSource;
        }

        public ZoomImagePopUp(string imageURL)
        {
            InitializeComponent();
            ViewImagePopUp.Source = imageURL;
        }

        public ZoomImagePopUp(byte[] byteImg)
        {
            InitializeComponent();
            GetSourceFromByte(byteImage: byteImg);
        }

        private void GetSourceFromByte(byte[] byteImage)
        {
           var stream = new MemoryStream(byteImage);
            ViewImagePopUp.Source = ImageSource.FromStream(() => stream);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            CloseAllPopup();
            return false; 
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void TapBackToDetailPage(object sender, System.EventArgs e)
        {
            await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PopPopupAsync();
            });
        }

        private void SwipeBackToDetailPage(object sender, SwipedEventArgs e)
        {
            BackToDetailPageExe();
        }

        private async void BackToDetailPageExe()
        {
            if (ViewImagePopUp.Scale == 1)
            {
                await DeviceExtension.BeginInvokeOnMainThreadAsync(async () =>
                {
                    await Navigation.PopPopupAsync();
                });
            }
        }
    }
}